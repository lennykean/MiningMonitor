using System;
using System.Collections.Generic;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service.Alerts.Scanners
{
    public abstract class AlertScanner : IAlertScanner
    {
        public Period CalculateScanPeriod(AlertDefinition definition, TimeSpan? minDuration, DateTime scanTime)
        {
            if (scanTime - minDuration < definition.NeedsScanAfter)
            {
                if (scanTime - minDuration >= definition.NoScanBefore)
                {
                    return new ConcretePeriod(scanTime - (TimeSpan)minDuration, scanTime);
                }
                return new ConcretePeriod(definition.NeedsScanAfter, scanTime);
            }
            return new ConcretePeriod(definition.NeedsScanAfter, scanTime);
        }

        public abstract Period CalculateScanPeriod(AlertDefinition definition, DateTime scanTime);

        public abstract bool EndAlert(AlertDefinition definition, Miner miner, Alert alert, IEnumerable<Snapshot> snapshots, DateTime scanTime);
        public abstract ScanResult PerformScan(IEnumerable<Alert> activeAlerts, AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime);
        public abstract bool ShouldScan(AlertDefinition definition);
    }
}
