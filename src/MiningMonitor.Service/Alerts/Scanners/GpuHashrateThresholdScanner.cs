﻿using System;
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

            return ScanResult.Fail(new[] {Alert.CreateFromDefinition(definition, definition.Parameters.AlertMessage ?? "GPU hashrate out of range")});
        }

        private static bool ShouldAlert(AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots)
        {
            if (!miner.CollectData)
                return false;

            var parameters = (GpuThresholdParameters)definition.Parameters;

            return (
                from snapshot in snapshots
                from gpu in snapshot.MinerStatistics.Gpus
                where gpu.EthereumHashrate / 1000m < parameters.MinValue || gpu.EthereumHashrate / 1000m > parameters.MaxValue
                select gpu).Any();
        }
    }
}
