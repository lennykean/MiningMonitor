using System.Collections.Generic;

namespace MiningMonitor.Alerts
{
    public class GpuCondition
    {
        public int GpuIndex { get; set; }
        public IEnumerable<GpuConditionPeriod> Periods { get; set; }
    }
}