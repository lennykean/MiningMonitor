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

        public ConcretePeriod CalculateScanPeriod(AlertDefinition definition, DateTime scanTime)
        {
            var durationMinutes = (definition.Parameters as ConnectivityAlertParameters)?.DurationMinutes;
            var duration = durationMinutes != null
                ? TimeSpan.FromMinutes((int)durationMinutes)
                : default(TimeSpan?);
            
            var start = scanTime - duration < definition.LastScan 
                ? scanTime - (TimeSpan)duration 
                : definition.LastScanEnd;

            return new ConcretePeriod(start, scanTime);
        }

        public bool EndAlert(AlertDefinition definition, Miner miner, Alert alert, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            return !ShouldAlert(definition, miner, snapshots, scanTime);
        }

        public Alert PerformScan(AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            return ShouldAlert(definition, miner, snapshots, scanTime) ? Alert.CreateFromDefinition(definition, definition.Parameters.AlertMessage ?? "No Connectivity") : null;
        }

        Period IAlertScanner.CalculateScanPeriod(AlertDefinition definition, DateTime scanTime)
        {
            return CalculateScanPeriod(definition, scanTime);
        }

        private bool ShouldAlert(AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            if (!miner.CollectData)
                return false;

            var scanRange = CalculateScanPeriod(definition, scanTime);
            var durationMinutes = (definition.Parameters as ConnectivityAlertParameters)?.DurationMinutes;
            var duration = durationMinutes != null ? TimeSpan.FromMinutes((int)durationMinutes) : default(TimeSpan?);

            var indexedSnapshotTimes = snapshots
                .Select(snapshot => snapshot.SnapshotTime)
                .Union(new [] {scanRange.Start, scanRange.End})
                .OrderBy(snapshotTime => snapshotTime)
                .Select((snapshotTime, index) => new {snapshotTime, index})
                .ToList();

            var snapshotGaps =
                from s1 in indexedSnapshotTimes
                join s2 in indexedSnapshotTimes on s1.index + 1 equals s2.index
                select new ConcretePeriod(s1.snapshotTime, s2.snapshotTime);

            return snapshotGaps.Any(gap => gap.Duration >= duration);
        }
    }
}
