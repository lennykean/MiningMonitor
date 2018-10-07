using System;

namespace MiningMonitor.Scheduler
{
    public class SnapshotDataCollectorSchedule : ISchedule
    {
        public bool DoWorkOnStartup { get; set; }
        public TimeSpan Interval { get; set; }
    }
}