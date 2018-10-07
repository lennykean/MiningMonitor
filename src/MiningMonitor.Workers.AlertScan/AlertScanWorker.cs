using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

using MiningMonitor.Alerts;
using MiningMonitor.Common;
using MiningMonitor.Model;
using MiningMonitor.Service;

namespace MiningMonitor.Workers.AlertScan
{
    public class AlertScanWorker : SynchronousWorker
    {
        private readonly IAlertDefinitionService _alertDefinitionService;
        private readonly IAlertService _alertService;
        private readonly ISnapshotService _snapshotService;
        private readonly IMinerService _minerService;
        private readonly IScanFactory _scanFactory;
        private readonly ILogger<AlertScanWorker> _logger;

        public AlertScanWorker(
            IAlertDefinitionService alertDefinitionService, 
            IAlertService alertService, 
            ISnapshotService snapshotService, 
            IMinerService minerService,
            IScanFactory scanFactory, 
            ILogger<AlertScanWorker> logger)
        {
            _alertDefinitionService = alertDefinitionService;
            _alertService = alertService;
            _snapshotService = snapshotService;
            _minerService = minerService;
            _scanFactory = scanFactory;
            _logger = logger;
        }

        protected override void DoWork()
        {
            _logger.LogInformation("Starting alert scan");
            try
            {
                var scanTime = DateTime.UtcNow;
                var scansByMiner =
                    from definition in _alertDefinitionService.GetEnabled()
                    group definition by definition.MinerId into definitions
                    let miner = _minerService.GetById(definitions.Key)
                    select new
                    {
                        Miner = miner,
                        Scans = definitions.Select(d => _scanFactory.CreateScan(d, miner, scanTime)).ToList()
                    };

                foreach (var scans in scansByMiner)
                {
                    var scanPeriod = Period.Merge(scans.Scans.Select(s => s.ScanPeriod));
                    var snapshots = _snapshotService.GetByMiner(scans.Miner.Id, scanPeriod).ToList();

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
   
            foreach (var scan in scans)
            {
                _logger.LogInformation($"Starting scan for alert definition {scan.Definition.Id}");
                try
                {
                    CheckActiveAlerts(miner, snapshots, scan);
                    ScanForNewAlerts(miner, snapshots, scan, scanTime);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error scanning for alert definition {scan.Definition.Id}");
                }
                _logger.LogInformation($"Finished scan for alert definition {scan.Definition.Id}");
            }
            _logger.LogInformation($"Finished scan of miner {miner.Id}");
        }

        private void ScanForNewAlerts(Miner miner, IEnumerable<Snapshot> snapshots, IScan scan, DateTime scanTime)
        {
            var activeAlerts = _alertService.GetActiveByDefinition(scan.Definition.Id, since: scan.Definition.LastEnabled).ToList();

            var result = scan.PerformScan(activeAlerts, snapshots);

            if (result.Skipped)
            {
                _logger.LogInformation($"Skipped scan for miner {miner.Id} definition {scan.Definition.Id}");
            }
            if (result.Failed)
            {
                _logger.LogInformation($"Creating {result.Alerts.Count()} alert(s) for miner {miner.Id} definition {scan.Definition.Id}");

                foreach (var alert in result.Alerts)
                {
                    _alertService.Add(alert);
                }
            }
            _alertDefinitionService.MarkScanned(scan.Definition.Id, scanTime);
        }

        private void CheckActiveAlerts(Miner miner, IEnumerable<Snapshot> snapshots, IScan scan)
        {
            var snapshotsForDefinition = snapshots.Where(s => scan.ScanPeriod.Contains(s.SnapshotTime)).ToList();
            var activeAlerts = _alertService.GetActiveByDefinition(scan.Definition.Id, since: scan.Definition.LastEnabled).ToList();

            foreach (var activeAlert in activeAlerts)
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
        }
    }
}