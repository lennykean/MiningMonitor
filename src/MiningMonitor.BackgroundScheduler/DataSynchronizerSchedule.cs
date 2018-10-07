using System;

namespace MiningMonitor.BackgroundScheduler
{
    public class DataSynchronizerSchedule : ISchedule
    {
        public bool DoWorkOnStartup { get; set; }
        public TimeSpan Interval { get; set; }
    }
}