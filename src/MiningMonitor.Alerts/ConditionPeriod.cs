using System.Collections.Generic;

using MiningMonitor.Common;
using MiningMonitor.Model;

namespace MiningMonitor.Alerts
{
    public class ConditionPeriod
    {
        public Condition Condition { get; set; }
        public Period Period { get; set; }
        public IList<Snapshot> Snapshots { get; set; }
    }
}