using System.Collections.Generic;

namespace MiningMonitor.Model.Alerts
{
    public class GpuThresholdPeriods
    {
        public int GpuIndex { get; set; }
        public IEnumerable<GpuThresholdPeriod> StatePeriods { get; set; }
    }
}