using System.Collections.Generic;
using System.Linq;

using ClaymoreMiner.RemoteManagement.Models;

using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts
{
    public class GpuConditionPeriod : ConditionPeriod
    {
        public int GpuIndex { get; set; }
        public Metric Metric { get; set; }
        public IEnumerable<GpuStats> GpuStats =>
            from s in Snapshots.Select((snapshot, index) => new {snapshot, index})
            where s.snapshot.MinerStatistics.Gpus.Count > GpuIndex
            select s.snapshot.MinerStatistics.Gpus[GpuIndex];
    }
}