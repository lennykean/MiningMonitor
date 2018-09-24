using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;
using MiningMonitor.Service;
using MiningMonitor.Service.Alerts;

namespace MiningMonitor.BackgroundWorker.Alerts
{
    public class AlertScan : IBackgroundWorker
    {
        private readonly IAlertDefinitionService _alertDefinitionService;
        private readonly IAlertService _alertService;
        private readonly ISnapshotService _snapshotService;
        private readonly IEnumerable<IAlertScanner> _scanners;
        private readonly ILogger<AlertScan> _logger;

        public AlertScan(IAlertDefinitionService alertDefinitionService, IAlertService alertService, ISnapshotService snapshotService, IEnumerable<IAlertScanner> scanners, ILogger<AlertScan> logger)
        {
            _alertDefinitionService = alertDefinitionService;
            _alertService = alertService;
            _snapshotService = snapshotService;
            _scanners = scanners;
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
                var definitionsByMiner =
                    from definition in _alertDefinitionService.GetEnabled()
                    group definition by definition.MinerId into g
                    select new { MinerId = g.Key, Definitions = g.ToList() };

                foreach (var definitions in definitionsByMiner)
                {
                    var scanStart = definitions.Definitions.Min(a => a.NextScanStartTime);
                    var snapshots = _snapshotService.GetByMiner(definitions.MinerId, from: scanStart, to: scanTime, fillGaps: false).ToList();

                    ScanMiner(scanTime, definitions.MinerId, definitions.Definitions, snapshots);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing alert scan");
            }
            _logger.LogInformation("Finished alert scan");
        }

        private void ScanMiner(DateTime scanTime, Guid minerId, IEnumerable<AlertDefinition> definitions, IList<Snapshot> snapshots)
        {
            _logger.LogInformation($"Starting scan of miner {minerId}");

            foreach (var definition in definitions)
            {
                _logger.LogInformation($"Starting scan for alert definition {definition.Id}");
                try
                {
                    var scanner = _scanners.SingleOrDefault(s => s.ShouldScan(definition));
                    if (scanner == null)
                        continue;

                    var snapshotsForDefinition = snapshots.Where(s => s.SnapshotTime > definition.NextScanStartTime);
                    var activeAlert = _alertService.GetLatestActiveByDefinition(definition.Id, since: definition.LastEnabled);
                    var (alert, description) = scanner.Scan(definition, snapshotsForDefinition);

                    if (alert && activeAlert == null)
                    {
                        _logger.LogInformation($"Creating alert for miner {minerId} definition {definition.Id}");

                        _alertService.Add(new Alert
                        {
                            MinerId = minerId,
                            AlertDefinitionId = definition.Id,
                            Description = description
                        });
                    }
                    else if (!alert && activeAlert != null)
                    {
                        _logger.LogInformation($"Ending alert {activeAlert.Id} for miner {minerId} with definition {definition.Id}");
                        _alertService.End(activeAlert.Id);
                    }
                    else if (alert)
                    {
                        _logger.LogInformation($"Continuing alert {activeAlert.Id} for miner {minerId} with definition {definition.Id}");
                    }
                    _alertDefinitionService.MarkScanned(definition, scanTime);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error scanning for alert definition {definition.Id}");
                }
                _logger.LogInformation($"Finished scan for alert definition {definition.Id}");
            }
            _logger.LogInformation($"Finished scan of miner {minerId}");
        }
    }
}