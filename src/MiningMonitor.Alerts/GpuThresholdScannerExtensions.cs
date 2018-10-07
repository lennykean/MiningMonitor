using System;
using System.Collections.Generic;
using System.Linq;

using ClaymoreMiner.RemoteManagement.Models;

using MiningMonitor.Common;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts
{
    public static class GpuThresholdScannerExtensions
    {
        public static IEnumerable<GpuThresholdPeriods> SelectGpuThresholdStatePeriods(this IEnumerable<Snapshot> snapshots, Func<GpuStats, GpuThresholdState> stateSelector)
        {
            var orderedSnapshots = snapshots.OrderBy(s => s.SnapshotTime).ToList();
            if (!orderedSnapshots.Any())
                yield break;

            var maxGpus = orderedSnapshots.Max(gpu => gpu.MinerStatistics.Gpus.Count);
            for (var i = 0; i < maxGpus; i++)
            {
                yield return new GpuThresholdPeriods
                {
                    GpuIndex = i,
                    StatePeriods = orderedSnapshots.SelectThresholdStatePeriods(i, stateSelector)
                };
            }
        }

        private static IEnumerable<GpuThresholdPeriod> SelectThresholdStatePeriods(this IEnumerable<Snapshot> snapshots, int gpuIndex, Func<GpuStats, GpuThresholdState> stateSelector)
        {
            var currentPeriod = default(GpuThresholdPeriod);

            foreach (var gpuStats in 
                from snapshot in snapshots
                where snapshot.MinerStatistics.Gpus.Count > gpuIndex
                select new {snapshot, gpuStats = snapshot.MinerStatistics.Gpus[gpuIndex]})
            {
                var state = stateSelector(gpuStats.gpuStats);

                if (state == currentPeriod?.State)
                {
                    currentPeriod.Snapshots.Add(gpuStats.snapshot);
                }
                else
                {
                    if (currentPeriod != null)
                    {
                        currentPeriod.Period = new Period(currentPeriod.Period.Start, gpuStats.snapshot.SnapshotTime);

                        yield return currentPeriod;
                    }
                    currentPeriod = new GpuThresholdPeriod
                    {
                        GpuIndex = gpuIndex,
                        State = state,
                        Period = new Period(gpuStats.snapshot.SnapshotTime, null),
                        Snapshots = new List<Snapshot> {gpuStats.snapshot}
                    };
                }
            }
            if (currentPeriod != null)
            {
                yield return currentPeriod;
            }
        }
    }
}

