using System;
using System.Collections.Generic;

using MiningMonitor.BackgroundScheduler;
using MiningMonitor.Common;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts.Scanners
{
    public abstract class AlertScanner : IAlertScanner
    {
        protected AlertScanner(DataCollectorSchedule dataCollectorSchedule)
        {
            DataCollectorSchedule = dataCollectorSchedule;
        }

        protected DataCollectorSchedule DataCollectorSchedule { get; }

        public Period CalculateScanPeriod(AlertDefinition definition, TimeSpan? minDuration, DateTime scanTime)
        {
            var paddedDuration = minDuration + DataCollectorSchedule.Interval + DataCollectorSchedule.Interval;

            if (scanTime - paddedDuration < definition.NeedsScanAfter)
            {
                if (scanTime - paddedDuration >= definition.NoScanBefore)
                {
                    return new ConcretePeriod(scanTime - (TimeSpan)paddedDuration, scanTime);
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
