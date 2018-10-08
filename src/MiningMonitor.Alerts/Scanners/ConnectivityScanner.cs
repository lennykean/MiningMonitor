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

            return !ShouldAlert(definition, snapshotsList, scanTime);
        }

        public override ScanResult PerformScan(IEnumerable<Alert> activeAlerts, AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            if (!miner.CollectData)
                return ScanResult.Skip;
            if (activeAlerts.Any())
                return ScanResult.Success;
            if (ShouldAlert(definition, snapshots, scanTime))
                return ScanResult.Fail(CreateAlert(definition));

            return ScanResult.Success;
        }

        private bool ShouldAlert(AlertDefinition definition, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            var scanRange = CalculateScanPeriod(definition, scanTime);
            var duration = TimeSpan.FromMinutes(((ConnectivityAlertParameters)definition.Parameters).DurationMinutes ?? 1);

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

            var lastGap = snapshotGaps.Last();

            return lastGap.Duration >= duration;
        }

        private static Alert CreateAlert(AlertDefinition definition)
        {
            var alert = Alert.CreateFromDefinition(definition, definition.Parameters.AlertMessage ?? "No Connectivity");

            alert.DetailMessages = new[] {$"No connection with miner for more than {definition.Parameters.DurationMinutes} minute(s)"};
                
            return alert;
        }
    }
}
