using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MiningMonitor.Service;

namespace MiningMonitor.BackgroundWorker.DataCollector
{
    public class DataSyncronizer : IBackgroundWorker
    {
        private readonly ISettingsService _settingsService;
        private readonly IMinerService _minerService;
        private readonly ISnapshotService _snapshotService;
        private readonly IServerService _serverService;
        private readonly ILogger<DataSyncronizer> _logger;

        public DataSyncronizer(ISettingsService settingsService, IMinerService minerService, ISnapshotService snapshotService, IServerService serverService, ILogger<DataSyncronizer> logger)
        {
            _settingsService = settingsService;
            _minerService = minerService;
            _snapshotService = snapshotService;
            _serverService = serverService;
            _logger = logger;
        }

        public async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            var (isDataCollector, registered, approved, id) = await Registration();
            if (!isDataCollector)
                return;
                    
            _logger.LogInformation("Starting Data Sync");
            if (registered && approved)
            {
                await SyncMiners(id);
                await SyncSnapshots(id);
            }
            else
            {
                _logger.LogInformation($"Not ready to sync data, registered: {registered}, approved: {approved}");
            }
        }

        private async Task SyncSnapshots(string id)
        {
            try
            {
                _logger.LogInformation("Starting snapshot sync");

                var snapshots = _snapshotService.GetAll();

                foreach (var snapshot in snapshots.ToList())
                {
                    _logger.LogInformation($"Syncing snapshot {snapshot.Id}");
                    await _serverService.SyncSnapshotAsync(id, snapshot);

                    _logger.LogInformation($"Removing snapshot {snapshot.Id}");
                    _snapshotService.Delete(snapshot.Id);
                }
                _logger.LogInformation("Finished snapshot sync");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing snapshots");
                throw;
            }
        }

        private async Task SyncMiners(string id)
        {
            try
            {
                _logger.LogInformation("Starting miner sync");

                var miners = _minerService.GetAll();

                foreach (var miner in miners.Where(m => m.IsSynced != true).ToList())
                {
                    _logger.LogInformation($"Syncing miner {miner.Id}");
                    await _serverService.SyncMinerAsync(id, miner);
                    _minerService.SetSynced(miner);
                }
                _logger.LogInformation("Finished miner sync");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing miners");
                throw;
            }
        }

        private async Task<(bool isDataCollector, bool registered, bool approved, string id)> Registration()
        {
            try
            {
                var (_, isDataCollectorSetting) = _settingsService.GetSetting("IsDataCollector");

                if (!bool.TryParse(isDataCollectorSetting, out var isDataCollector) || !isDataCollector)
                    return (isDataCollector, registered: false, approved: false, id: null);

                _logger.LogInformation("Checking for registration");
                var (_, id) = _settingsService.GetSetting("CollectorId");
                if (id == null)
                {
                    string token;

                    _logger.LogInformation("Registering as data collector");
                    (id, token) = await _serverService.RegisterAsCollectorAsync();

                    _settingsService.UpdateSetting("CollectorId", id);
                    _settingsService.UpdateSetting("ServerToken", token);
                }

                _logger.LogInformation("Checking approval");
                var approved = await _serverService.CheckApprovalAsync(id);

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