using System;
using System.Collections.Generic;
using System.Linq;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service.Alerts.Scanners
{
    public class ConnectivityScanner : IAlertScanner
    {
        public bool ShouldScan(AlertDefinition definition)
        {
            return definition.Parameters.AlertType == AlertType.Connectivity;
        }

        public (DateTime start, DateTime end) CalculateScanRange(AlertDefinition definition, DateTime scanTime)
        {
            var durationMinutes = (definition.Parameters as ConnectivityAlertParameters)?.DurationMinutes;
            var duration = durationMinutes != null
                ? TimeSpan.FromMinutes((int)durationMinutes)
                : default(TimeSpan?);
            
            var start = scanTime - duration < definition.LastScan 
                ? scanTime - (TimeSpan)duration 
                : definition.LastScanEnd;

            return (start, scanTime);
        }

        public bool EndAlert(AlertDefinition definition, Miner miner, Alert alert, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            return !ShouldAlert(definition, miner, snapshots, scanTime);
        }

        public Alert PerformScan(AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            if (!ShouldAlert(definition, miner, snapshots, scanTime))
                return null;

            return Alert.CreateFromDefinition(definition, definition.Parameters.AlertMessage ?? "No connectivity");
        }

        private bool ShouldAlert(AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            if (!miner.CollectData)
                return false;

            var scanRange = CalculateScanRange(definition, scanTime);
            var durationMinutes = (definition.Parameters as ConnectivityAlertParameters)?.DurationMinutes;
            var duration = durationMinutes != null ? TimeSpan.FromMinutes((int)durationMinutes) : default(TimeSpan?);

            var indexedSnapshotTimes = snapshots
                .Select(snapshot => snapshot.SnapshotTime)
                .Union(new [] {scanRange.start, scanRange.end})
                .OrderBy(snapshotTime => snapshotTime)
                .Select((snapshotTime, index) => new {snapshotTime, index})
                .ToList();

            var snapshotGaps =
                from s1 in indexedSnapshotTimes
                join s2 in indexedSnapshotTimes on s1.index + 1 equals s2.index
                select new {s1.snapshotTime, next = s2.snapshotTime};

            return snapshotGaps.Any(gap => gap.next - gap.snapshotTime >= duration);
        }
    }
}
