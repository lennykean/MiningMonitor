using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MiningMonitor.Model;
using MiningMonitor.Service;
using MiningMonitor.Service.Alerts;

namespace MiningMonitor.BackgroundWorker.Alerts
{
    public class AlertScan : IBackgroundWorker
    {
        private readonly IAlertDefinitionService _alertDefinitionService;
        private readonly IAlertService _alertService;
        private readonly ISnapshotService _snapshotService;
        private readonly IMinerService _minerService;
        private readonly IScanFactory _scanFactory;
        private readonly ILogger<AlertScan> _logger;

        public AlertScan(
            IAlertDefinitionService alertDefinitionService, 
            IAlertService alertService, 
            ISnapshotService snapshotService, 
            IMinerService minerService,
            IScanFactory scanFactory, 
            ILogger<AlertScan> logger)
        {
            _alertDefinitionService = alertDefinitionService;
            _alertService = alertService;
            _snapshotService = snapshotService;
            _minerService = minerService;
            _scanFactory = scanFactory;
            _logger = logger;
        }

        public async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => DoWork(), cancellationToken);
        }

        public void DoWork()
        {
            _logger.LogInformation("Starting alert scan");
            try
            {
                var scanTime = DateTime.UtcNow;
                var scansByMiner =
                    from definition in _alertDefinitionService.GetEnabled()
                    let scan = _scanFactory.CreateScan(definition)
                    group scan by definition.MinerId into grouping
                    let miner = _minerService.GetById(grouping.Key)
                    select new { Miner = miner, Scans = grouping.ToList() };

                foreach (var scans in scansByMiner)
                {
                    var scanStart = scans.Scans.Min(s => s.ScanStart);
                    var snapshots = _snapshotService.GetByMiner(scans.Miner.Id, from: scanStart, to: scanTime, fillGaps: false).ToList();

                    ScanMiner(scanTime, scans.Miner, scans.Scans, snapshots);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing alert scan");
            }
            _logger.LogInformation("Finished alert scan");
        }

        private void ScanMiner(DateTime scanTime, Miner miner, IEnumerable<IScan> scans, IList<Snapshot> snapshots)
        {
            _logger.LogInformation($"Starting scan of miner {miner.Id}");

            if (miner?.CollectData != true) {
                _logger.LogInformation($"Miner {miner.Id} is not collecting data, skipping scan");
                return;
            }            
            foreach (var scan in scans)
            {
                _logger.LogInformation($"Starting scan for alert definition {scan.Definition.Id}");
                try
                {
                    var snapshotsForDefinition = snapshots.Where(s => s.SnapshotTime > scan.ScanStart);
                    var activeAlert = _alertService.GetLatestActiveByDefinition(scan.Definition.Id, since: scan.Definition.LastEnabled);
                    if (activeAlert == null)
                    {
                        var alert = scan.PerformScan(snapshots);
                        if (alert != null)
                        {
                            _logger.LogInformation($"Creating alert for miner {miner.Id} definition {scan.Definition.Id}");
                            _alertService.Add(alert);
                        }
                    }
                    else
                    {
                        if (scan.EndAlert(activeAlert, snapshotsForDefinition))
                        {
                            _logger.LogInformation($"Ending alert {activeAlert.Id} for miner {miner.Id} with definition {scan.Definition.Id}");
                            activeAlert.End = DateTime.UtcNow;
                        }
                        else
                        {
                            _logger.LogInformation($"Continuing alert {activeAlert.Id} for miner {miner.Id} with definition {scan.Definition.Id}");
                            activeAlert.LastActive = DateTime.UtcNow;
                        }
                        _alertService.Update(activeAlert);
                    }
                    _alertDefinitionService.MarkScanned(scan.Definition.Id, scanTime);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error scanning for alert definition {scan.Definition.Id}");
                }
                _logger.LogInformation($"Finished scan for alert definition {scan.Definition.Id}");
            }
            _logger.LogInformation($"Finished scan of miner {miner.Id}");
        }
    }
}