using System;

namespace MiningMonitor.Common
{
    public static class TimeSpanExtensions
    {
        public static TimeSpan? MinutesToTimeSpan(this int? minutes)
        {
            if (minutes == null)
                return null;

            return TimeSpan.FromMinutes((int)minutes);
        }
    }
}
