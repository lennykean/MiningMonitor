using System;

namespace MiningMonitor.Scheduler
{
    public class PurgeSchedule : ISchedule
    {
        public bool DoWorkOnStartup { get; set; }
        public TimeSpan Interval { get; set; }
    }
}