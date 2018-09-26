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

        public DateTime CalculateScanStart(AlertDefinition definition)
        {
            return definition.NextScanStartTime;
        }

        public bool EndAlert(AlertDefinition definition, Alert alert, IEnumerable<Snapshot> snapshots)
        {
            return !ShouldAlert(definition, snapshots);
        }

        public Alert PerformScan(AlertDefinition definition, IEnumerable<Snapshot> snapshots)
        {
            if (!ShouldAlert(definition, snapshots))
                return null;

            return Alert.CreateFromDefinition(definition, definition.CustomMessage ?? "Hashrate too low");
        }

        private static bool ShouldAlert(AlertDefinition definition, IEnumerable<Snapshot> snapshots)
        {
            var minHashrate = ((HashrateAlertParameters)definition.Parameters).MinValue;
            
            return snapshots.Any(s => s.MinerStatistics.Ethereum.Hashrate / 1000m < minHashrate);
        }
    }
}
