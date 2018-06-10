using System;

using MiningMonitor.BackgroundWorker.Scheduler;

namespace MiningMonitor.BackgroundWorker.Maintenance
{
    public class PurgeSchedule : ISchedule
    {
        public bool DoWorkOnStartup { get; set; }
        public TimeSpan Interval { get; set; }
    }
}