using System;

namespace MiningMonitor.BackgroundScheduler
{
    public class AlertScanSchedule : ISchedule
    {
        public bool DoWorkOnStartup { get; set; }
        public TimeSpan Interval { get; set; }
    }
}