using System;

namespace MiningMonitor.BackgroundScheduler
{
    public class DataCollectorSchedule : ISchedule
    {
        public bool DoWorkOnStartup { get; set; }
        public TimeSpan Interval { get; set; }
    }
}