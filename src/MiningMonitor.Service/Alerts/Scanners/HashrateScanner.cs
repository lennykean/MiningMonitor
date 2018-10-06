using System;
using System.Collections.Generic;
using System.Linq;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service.Alerts.Scanners
{
    public class HashrateScanner : IAlertScanner
    {
        public bool ShouldScan(AlertDefinition definition)
        {
            return definition.Parameters.AlertType == AlertType.Hashrate;
        }

        public Period CalculateScanPeriod(AlertDefinition definition, DateTime scanTime)
        {
            return new Period(definition.NeedsScanAfter, scanTime);
        }

        public bool EndAlert(AlertDefinition definition, Miner miner, Alert alert, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            var snapshotsList = snapshots.ToList();

            if (!snapshotsList.Any())
                return false;

            return !ShouldAlert(definition, miner, snapshotsList);
        }

        public ScanResult PerformScan(AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            if (!ShouldAlert(definition, miner, snapshots))
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
