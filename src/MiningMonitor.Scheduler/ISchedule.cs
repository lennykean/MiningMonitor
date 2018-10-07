using System;

namespace MiningMonitor.Scheduler
{
    public interface ISchedule
    {
        bool DoWorkOnStartup { get; }
        TimeSpan Interval { get; }
    }
}