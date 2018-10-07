using System;

namespace MiningMonitor.Scheduler
{
    public class AlertScanSchedule : ISchedule
    {
        public bool DoWorkOnStartup { get; set; }
        public TimeSpan Interval { get; set; }
    }
}