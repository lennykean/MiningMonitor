using System;

using MiningMonitor.BackgroundWorker.Scheduler;

namespace MiningMonitor.BackgroundWorker.Alerts
{
    public class AlertScanSchedule : ISchedule
    {
        public bool DoWorkOnStartup { get; set; }
        public TimeSpan Interval { get; set; }
    }
}