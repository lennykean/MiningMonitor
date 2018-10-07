using System;
using System.Collections.Generic;
using System.Linq;

using MiningMonitor.Common;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts.Scanners
{
    public class ConnectivityScanner : AlertScanner
    {
        public override bool ShouldScan(AlertDefinition definition)
        {
            return definition.Parameters.AlertType == AlertType.Connectivity;
        }
        
        public override bool EndAlert(AlertDefinition definition, Miner miner, Alert alert, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            var snapshotsList = snapshots.ToList();

            if (!snapshotsList.Any())
                return false;

            return !ShouldAlert(definition, miner, snapshotsList, scanTime);
        }

        public override ScanResult PerformScan(IEnumerable<Alert> activeAlerts, AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            if (activeAlerts.Any() || !ShouldAlert(definition, miner, snapshots, scanTime))
                return ScanResult.Success;

            return ScanResult.Fail(new [] {Alert.CreateFromDefinition(definition, definition.Parameters.AlertMessage ?? "No Connectivity")});
        }

        private bool ShouldAlert(AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            if (!miner.CollectData)
                return false;

            var scanRange = CalculateScanPeriod(definition, scanTime);
            var durationMinutes = ((ConnectivityAlertParameters)definition.Parameters).DurationMinutes;
            var duration = durationMinutes != null ? TimeSpan.FromMinutes((int)durationMinutes) : default(TimeSpan?);

            var indexedSnapshotTimes = snapshots
                .Select(snapshot => (DateTime?)snapshot.SnapshotTime)
                .Union(new [] {scanRange.Start, scanRange.End})
                .OrderBy(snapshotTime => snapshotTime)
                .Select((snapshotTime, index) => new {snapshotTime, index})
                .ToList();

            var snapshotGaps =
                from s1 in indexedSnapshotTimes
                join s2 in indexedSnapshotTimes on s1.index + 1 equals s2.index
                select new Period(s1.snapshotTime, s2.snapshotTime);

            return snapshotGaps.Any(gap => gap.Duration >= duration);
        }
    }
}
