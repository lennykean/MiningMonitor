using System;
using System.Collections.Generic;
using System.Linq;

using ClaymoreMiner.RemoteManagement.Models;

using MiningMonitor.Common;
using MiningMonitor.Model;

namespace MiningMonitor.Alerts
{
    public static class ConditionMapperExtensions
    {

        public static IEnumerable<ConditionPeriod> ToConditionPeriods(this IEnumerable<Snapshot> snapshots, Func<Snapshot, Condition> conditionSelector)
        {
            var currentPeriod = default(ConditionPeriod);

            foreach (var snapshot in snapshots)
            {
                var condition = conditionSelector(snapshot);

                if (condition == currentPeriod?.Condition)
                {
                    currentPeriod.Snapshots.Add(snapshot);
                }
                else
                {
                    if (currentPeriod != null)
                    {
                        currentPeriod.Period = new Period(currentPeriod.Period.Start, snapshot.SnapshotTime);

                        yield return currentPeriod;
                    }
                    currentPeriod = new ConditionPeriod
                    {
                        Condition = condition,
                        Period = new Period(snapshot.SnapshotTime, null),
                        Snapshots = new List<Snapshot> { snapshot }
                    };
                }
            }
            if (currentPeriod != null)
            {
                yield return currentPeriod;
            }
        }

        public static IEnumerable<GpuCondition> ToGpuConditions(this IEnumerable<Snapshot> snapshots, Func<GpuStats, Condition> conditionSelector)
        {
            var orderedSnapshots = snapshots.OrderBy(s => s.SnapshotTime).ToList();
            if (!orderedSnapshots.Any())
                yield break;

            var maxGpus = orderedSnapshots.Max(gpu => gpu.MinerStatistics.Gpus.Count);
            for (var i = 0; i < maxGpus; i++)
            {
                yield return new GpuCondition
                {
                    GpuIndex = i,
                    Periods = orderedSnapshots.ToGpuConditionPeriods(i, conditionSelector)
                };
            }
        }

        private static IEnumerable<GpuConditionPeriod> ToGpuConditionPeriods(this IEnumerable<Snapshot> snapshots, int gpuIndex, Func<GpuStats, Condition> conditionSelector)
        {
            var currentPeriod = default(GpuConditionPeriod);

            foreach (var gpuSnapshot in 
                from snapshot in snapshots
                where snapshot.MinerStatistics.Gpus.Count > gpuIndex
                select new {snapshot, gpuStats = snapshot.MinerStatistics.Gpus[gpuIndex]})
            {
                var condition = conditionSelector(gpuSnapshot.gpuStats);

                if (condition == currentPeriod?.Condition)
                {
                    currentPeriod.Snapshots.Add(gpuSnapshot.snapshot);
                }
                else
                {
                    if (currentPeriod != null)
                    {
                        currentPeriod.Period = new Period(currentPeriod.Period.Start, gpuSnapshot.snapshot.SnapshotTime);

                        yield return currentPeriod;
                    }
                    currentPeriod = new GpuConditionPeriod
                    {
                        GpuIndex = gpuIndex,
                        Condition = condition,
                        Period = new Period(gpuSnapshot.snapshot.SnapshotTime, null),
                        Snapshots = new List<Snapshot> {gpuSnapshot.snapshot}
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

