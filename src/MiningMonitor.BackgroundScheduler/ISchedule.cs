using System;

namespace MiningMonitor.BackgroundScheduler
{
    public interface ISchedule
    {
        bool DoWorkOnStartup { get; }
        TimeSpan Interval { get; }
    }
}