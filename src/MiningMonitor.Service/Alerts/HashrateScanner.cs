using System.Collections.Generic;
using System.Linq;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service.Alerts
{
    public class HashrateScanner : IAlertScanner
    {
        public bool ShouldScan(AlertDefinition definition)
        {
            return definition.Parameters.AlertType == AlertType.Hashrate;
        }

        public (bool alert, string description) Scan(AlertDefinition definition, IEnumerable<Snapshot> snapshots)
        {
            string description = null;

            var minHashrate = ((HashrateAlertParameters)definition.Parameters).MinValue;
            var alert = snapshots.Any(s => s.MinerStatistics?.Ethereum.Hashrate / 1000m < minHashrate);

            if (alert)
            {
                description = definition.CustomMessage ?? $"Hashrate below minimum value of {minHashrate} MH/s";
            }

            return (alert, description);
        }
    }
}
