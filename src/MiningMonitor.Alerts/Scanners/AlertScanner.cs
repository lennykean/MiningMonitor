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
        private readonly DataCollectorSchedule _dataCollectorSchedule;

        protected AlertScanner(DataCollectorSchedule dataCollectorSchedule)
        {
            _dataCollectorSchedule = dataCollectorSchedule;
        }

        public virtual Period CalculateScanPeriod(AlertDefinition definition, DateTime scanTime)
        {
            var duration = definition.Parameters.DurationMinutes.MinutesToTimeSpan();
            var paddedDuration = duration + _dataCollectorSchedule.Interval + _dataCollectorSchedule.Interval;

            return definition.NextScanPeriod(scanTime, paddedDuration);
        }

        public abstract bool ShouldScan(AlertDefinition definition);
        public abstract bool EndAlert(AlertDefinition definition, Miner miner, Alert alert, IEnumerable<Snapshot> snapshots, DateTime scanTime);
        public abstract ScanResult PerformScan(IEnumerable<Alert> activeAlerts, AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime);
    }
}
