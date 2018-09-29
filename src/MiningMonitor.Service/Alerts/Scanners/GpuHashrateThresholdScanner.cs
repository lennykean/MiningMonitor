using System;
using System.Collections.Generic;
using System.Linq;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;
using MiningMonitor.Service.Alerts.Scanners;

namespace MiningMonitor.Service.Alerts
{
    public class GpuHashrateThresholdScanner : IAlertScanner
    {
        public bool ShouldScan(AlertDefinition definition)
        {
            return definition.Parameters is GpuThresholdParameters parameters && parameters.Metric == Metric.Hashrate;
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

            return Alert.CreateFromDefinition(definition, definition.Parameters.AlertMessage ?? "GPU hashrate out of range");
        }

        private static bool ShouldAlert(AlertDefinition definition, IEnumerable<Snapshot> snapshots)
        {
            var parameters = (GpuThresholdParameters)definition.Parameters;

            return (
                from snapshot in snapshots
                from gpu in snapshot.MinerStatistics.Gpus
                where gpu.EthereumHashrate / 1000m < parameters.MinValue || gpu.EthereumHashrate / 1000m > parameters.MaxValue
                select gpu).Any();
        }
    }
}
