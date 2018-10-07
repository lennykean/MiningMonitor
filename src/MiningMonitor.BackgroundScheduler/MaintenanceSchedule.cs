using System;

namespace MiningMonitor.BackgroundScheduler
{
    public class MaintenanceSchedule : ISchedule
    {
        public bool DoWorkOnStartup { get; set; }
        public TimeSpan Interval { get; set; }
    }
}