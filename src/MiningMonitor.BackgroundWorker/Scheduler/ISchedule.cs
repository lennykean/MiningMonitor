using System;

namespace MiningMonitor.BackgroundWorker.Scheduler
{
    public interface ISchedule
    {
        bool DoWorkOnStartup { get; }
        TimeSpan Interval { get; }
    }
}