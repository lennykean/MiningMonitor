using System;
using System.Collections.Generic;
using System.Linq;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;
using MiningMonitor.Service.Alerts.Scanners;

namespace MiningMonitor.Service.Alerts
{
    public class GpuFanSpeedThresholdScanner : IAlertScanner
    {
        public bool ShouldScan(AlertDefinition definition)
        {
            return definition.Parameters is GpuThresholdParameters parameters && parameters.Metric == Metric.FanSpeed;
        }

        public Period CalculateScanPeriod(AlertDefinition definition, DateTime scanTime)
        {
            return new Period(definition.LastScanEnd, scanTime);
        }

        public bool EndAlert(AlertDefinition definition, Miner miner, Alert alert, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            return !ShouldAlert(definition, miner, snapshots);
        }

        public Alert PerformScan(AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            if (!ShouldAlert(definition, miner, snapshots))
                return null;

            return Alert.CreateFromDefinition(definition, definition.Parameters.AlertMessage ?? "GPU fan speed out of range");
        }

        private static bool ShouldAlert(AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots)
        {
            if (!miner.CollectData)
                return false;

            var parameters = (GpuThresholdParameters)definition.Parameters;

            return (
                from snapshot in snapshots
                from gpu in snapshot.MinerStatistics.Gpus
                where gpu.FanSpeed < parameters.MinValue || gpu.FanSpeed > parameters.MaxValue
                select gpu).Any();
        }
    }
}
