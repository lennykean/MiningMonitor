using System.Collections.Generic;
using System.Linq;

using ClaymoreMiner.RemoteManagement.Models;

namespace MiningMonitor.Model.Alerts
{
    public class GpuThresholdPeriod
    {
        public int GpuIndex { get; set; }
        public GpuThresholdState State { get; set; }
        public Period Period { get; set; }
        public IList<Snapshot> Snapshots { get; set; }
        public IEnumerable<GpuStats> GpuStats =>
            from s in Snapshots.Select((snapshot, index) => new {snapshot, index})
            where s.snapshot.MinerStatistics.Gpus.Count > GpuIndex
            select s.snapshot.MinerStatistics.Gpus[GpuIndex];
    }
}