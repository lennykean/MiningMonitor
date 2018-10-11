using System;

using MiningMonitor.Workers;

namespace MiningMonitor.BackgroundScheduler
{
    public class Schedule<TWorker> where TWorker : IWorker
    {
        public bool DoWorkOnStartup { get; set; }
        public TimeSpan Interval { get; set; }
    }
}