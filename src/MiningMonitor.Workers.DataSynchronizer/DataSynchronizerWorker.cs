using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MiningMonitor.Service;

namespace MiningMonitor.Workers.DataSynchronizer
{
    public class DataSynchronizerWorker : IWorker
    {
        private readonly ISettingsService _settingsService;
        private readonly IMinerService _minerService;
        private readonly ISnapshotService _snapshotService;
        private readonly IServerService _serverService;
        private readonly ILogger<DataSynchronizerWorker> _logger;

        public DataSynchronizerWorker(ISettingsService settingsService, IMinerService minerService, ISnapshotService snapshotService, IServerService serverService, ILogger<DataSynchronizerWorker> logger)
        {
            _settingsService = settingsService;
            _minerService = minerService;
            _snapshotService = snapshotService;
            _serverService = serverService;
            _logger = logger;
        }

        public async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            var (isDataCollector, registered, approved, id) = await RegistrationAsync(cancellationToken);
            if (!isDataCollector)
                return;
                    
            _logger.LogInformation("Starting Data Sync");
            if (registered && approved)
            {
                await SyncMinersAsync(id, cancellationToken);
                await SyncSnapshotsAsync(id, cancellationToken);
            }
            else
            {
                _logger.LogInformation($"Not ready to sync data, registered: {registered}, approved: {approved}");
            }
        }

        private async Task SyncSnapshotsAsync(string id, CancellationToken token)
        {
            try
            {
                _logger.LogInformation("Starting snapshot sync");

                var snapshots = await _snapshotService.GetAllAsync(token);

                foreach (var snapshot in snapshots.ToList())
                {
                    _logger.LogInformation($"Syncing snapshot {snapshot.Id}");
                    await _serverService.SyncSnapshotAsync(id, snapshot, token);

                    _logger.LogInformation($"Removing snapshot {snapshot.Id}");
                    await _snapshotService.DeleteAsync(snapshot.Id, token);
                }
                _logger.LogInformation("Finished snapshot sync");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing snapshots");
                throw;
            }
        }

        private async Task SyncMinersAsync(string id, CancellationToken token)
        {
            try
            {
                _logger.LogInformation("Starting miner sync");

                var miners = await _minerService.GetAllAsync(token);

                foreach (var miner in miners.Where(m => m.IsSynced != true).ToList())
                {
                    _logger.LogInformation($"Syncing miner {miner.Id}");
                    await _serverService.SyncMinerAsync(id, miner, token);
                    await _minerService.SetSyncedAsync(miner, token);
                }
                _logger.LogInformation("Finished miner sync");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing miners");
                throw;
            }
        }

        private async Task<(bool isDataCollector, bool registered, bool approved, string id)> RegistrationAsync(CancellationToken cancellationToken)
        {
            try
            {
                var (_, isDataCollectorSetting) = await _settingsService.GetSettingAsync("IsDataCollector", cancellationToken);

                if (!bool.TryParse(isDataCollectorSetting, out var isDataCollector) || !isDataCollector)
                    return (isDataCollector, registered: false, approved: false, id: null);

                _logger.LogInformation("Checking for registration");
                var (_, id) = await _settingsService.GetSettingAsync("CollectorId", cancellationToken);
                if (id == null)
                {
                    string token;

                    _logger.LogInformation("Registering as data collector");
                    (id, token) = await _serverService.RegisterAsCollectorAsync(cancellationToken);

                    await _settingsService.UpdateSettingAsync("CollectorId", id, cancellationToken);
                    await _settingsService.UpdateSettingAsync("ServerToken", token, cancellationToken);
                }

                _logger.LogInformation("Checking approval");
                var approved = await _serverService.CheckApprovalAsync(id, cancellationToken);

                return (isDataCollector: true, registered: true, approved, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured during registration check");
                throw;
            }
        }
    }
}