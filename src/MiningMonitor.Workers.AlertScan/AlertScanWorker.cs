using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MiningMonitor.Alerts;
using MiningMonitor.Common;
using MiningMonitor.Model;
using MiningMonitor.Service;

namespace MiningMonitor.Workers.AlertScan
{
    public class AlertScanWorker : IWorker
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

        public async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting alert scan");
            try
            {
                var scanTime = DateTime.UtcNow;
                var scansByMiner =
                    from definition in await _alertDefinitionService.GetEnabledAsync(cancellationToken)
                    group definition by definition.MinerId into definitions
                    let miner = _minerService.GetByIdAsync(definitions.Key, cancellationToken).Result
                    select new
                    {
                        Miner = miner,
                        Scans = definitions.Select(d => _scanFactory.CreateScan(d, miner, scanTime)).ToList()
                    };

                foreach (var scans in scansByMiner)
                {
                    var scanPeriod = Period.Merge(scans.Scans.Select(s => s.ScanPeriod));
                    var snapshots = (await _snapshotService.GetByMinerAsync(scans.Miner.Id, scanPeriod, cancellationToken)).ToList();

                    await ScanMinerAsync(scanTime, scans.Miner, scans.Scans, snapshots, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing alert scan");
            }
            _logger.LogInformation("Finished alert scan");
        }

        private async Task ScanMinerAsync(DateTime scanTime, Miner miner, IEnumerable<IScan> scans, IList<Snapshot> snapshots, CancellationToken token)
        {
            _logger.LogInformation($"Starting scan of miner {miner.Id}");
   
            foreach (var scan in scans)
            {
                _logger.LogInformation($"Starting scan for alert definition {scan.Definition.Id}");
                try
                {
                    await CheckActiveAlerts(miner, snapshots, scan, token);
                    await ScanForNewAlertsAsync(miner, snapshots, scan, scanTime, token);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error scanning for alert definition {scan.Definition.Id}");
                }
                _logger.LogInformation($"Finished scan for alert definition {scan.Definition.Id}");
            }
            _logger.LogInformation($"Finished scan of miner {miner.Id}");
        }

        private async Task ScanForNewAlertsAsync(Miner miner, IEnumerable<Snapshot> snapshots, IScan scan, DateTime scanTime, CancellationToken token)
        {
            var activeAlerts = (await _alertService.GetActiveByDefinitionAsync(scan.Definition.Id, since: scan.Definition.LastEnabled, token: token)).ToList();

            var result = await scan.PerformScanAsync(activeAlerts, snapshots, token);

            if (result.Skipped)
            {
                _logger.LogInformation($"Skipped scan for miner {miner.Id} definition {scan.Definition.Id}");
            }
            if (result.Failed)
            {
                _logger.LogInformation($"Creating {result.Alerts.Count()} alert(s) for miner {miner.Id} definition {scan.Definition.Id}");

                foreach (var alert in result.Alerts)
                {
                    await _alertService.AddAsync(alert, token);
                }
            }
            await _alertDefinitionService.MarkScannedAsync(scan.Definition.Id, scanTime, token);
        }

        private async Task CheckActiveAlerts(Miner miner, IEnumerable<Snapshot> snapshots, IScan scan, CancellationToken token)
        {
            var snapshotsForDefinition = snapshots.Where(s => scan.ScanPeriod.Contains(s.SnapshotTime)).ToList();
            var activeAlerts = (await _alertService.GetActiveByDefinitionAsync(scan.Definition.Id, since: scan.Definition.LastEnabled, token: token)).ToList();

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
                await _alertService.UpdateAsync(activeAlert, token);
            }
        }
    }
}