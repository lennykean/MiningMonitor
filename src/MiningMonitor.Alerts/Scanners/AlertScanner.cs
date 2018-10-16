using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Common;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts.Scanners
{
    public abstract class AlertScanner : IAlertScanner
    {
        public virtual Period CalculateScanPeriod(AlertDefinition definition, DateTime scanTime)
        {
            var duration = definition.Parameters.DurationMinutes.MinutesToTimeSpan();
            var paddedDuration = duration + duration;

            return definition.NextScanPeriod(scanTime, paddedDuration);
        }

        public abstract bool ShouldScan(AlertDefinition definition);
        public abstract bool EndAlert(AlertDefinition definition, Miner miner, Alert alert, IEnumerable<Snapshot> snapshots, DateTime scanTime);
        public abstract Task<ScanResult> PerformScanAsync(IEnumerable<Alert> activeAlerts, AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime, CancellationToken token);
    }
}
