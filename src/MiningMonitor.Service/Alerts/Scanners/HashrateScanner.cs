using System;
using System.Collections.Generic;
using System.Linq;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service.Alerts.Scanners
{
    public class HashrateScanner : AlertScanner
    {
        public override bool ShouldScan(AlertDefinition definition)
        {
            return definition.Parameters.AlertType == AlertType.Hashrate;
        }

        public override Period CalculateScanPeriod(AlertDefinition definition, DateTime scanTime)
        {
            return new ConcretePeriod(definition.NeedsScanAfter, scanTime);
        }

        public override bool EndAlert(AlertDefinition definition, Miner miner, Alert alert, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            var snapshotsList = snapshots.ToList();

            if (!snapshotsList.Any())
                return false;

            return !ShouldAlert(definition, miner, snapshotsList);
        }

        public override ScanResult PerformScan(IEnumerable<Alert> activeAlerts, AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            if (activeAlerts.Any() || !ShouldAlert(definition, miner, snapshots))
                return ScanResult.Success;

            return ScanResult.Fail(new[] {Alert.CreateFromDefinition(definition, definition.Parameters.AlertMessage ?? "Hashrate too low")});
        }

        private static bool ShouldAlert(AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots)
        {
            if (!miner.CollectData)
                return false;

            var minHashrate = ((HashrateAlertParameters)definition.Parameters).MinValue;
            
            return snapshots.Any(s => s.MinerStatistics.Ethereum.Hashrate / 1000m < minHashrate);
        }
    }
}
