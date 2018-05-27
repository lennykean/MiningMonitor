using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MiningMonitor.Model;
using MiningMonitor.Service;

namespace MiningMonitor.BackgroundWorker.DataCollector
{
    public class SnapshotDataCollector : IBackgroundWorker
    {
        private readonly IMinerService _minerService;
        private readonly ISnapshotService _snapshotService;
        private readonly IRemoteManagementClientFactory _clientFactory;
        private readonly ILogger<SnapshotDataCollector> _logger;

        public SnapshotDataCollector(
            IMinerService minerService,
            ISnapshotService snapshotService,
            IRemoteManagementClientFactory clientFactory,
            ILogger<SnapshotDataCollector> logger)
        {
            _minerService = minerService;
            _snapshotService = snapshotService;
            _clientFactory = clientFactory;
            _logger = logger;
        }

        Task IBackgroundWorker.DoWorkAsync(CancellationToken cancellationToken) => Collect();

        public async Task Collect()
        {
            _logger.LogInformation("Starting snapshot collection");
            try
            {
                var miners = await _minerService.GetEnabledMinersAsync();

                foreach (var miner in miners.AsParallel())
                {
                    await GetSnapshot(miner);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error collecting snapshots");
            }

            _logger.LogInformation("Finished snapshot collection");
        }

        private async Task GetSnapshot(Miner miner)
        {
            _logger.LogInformation($"Getting statistics for {miner.Name}");
            try
            {
                var client = _clientFactory.Create(miner);

                var sw = Stopwatch.StartNew();
                var minerStatistics = await client.GetStatisticsAsync();
                sw.Stop();

                await _snapshotService.AddAsync(new Snapshot
                {
                    MinerId = miner.Id,
                    RetrievalElapsedTime = sw.Elapsed,
                    SnapshotTime = DateTime.UtcNow,
                    MinerStatistics = minerStatistics
                });
                _logger.LogInformation($"Saved statistics for {miner.Name} in {sw.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting statistics for miner {miner.Name}");
            }
        }
    }
}