using System;

namespace MiningMonitor.Common
{
    public struct ConcretePeriod
    {
        public ConcretePeriod(DateTime start, DateTime end)
        {
            if (end < start)
                throw new ArgumentException("end cannot be before start", nameof(end));

            Start = start;
            End = end;
        }
        
        public DateTime Start { get; }
        public DateTime End { get; }
        public TimeSpan Duration => End - Start;
        public bool IsInstant => Start == End;
        
        public override int GetHashCode()
        {
            unchecked
            {
                return (Start.GetHashCode() * 397) ^ End.GetHashCode();
            }
        }

        public bool Equals(ConcretePeriod other)
        {
            return Start.Equals(other.Start) && End.Equals(other.End);
        }

        public bool Equals(Period other)
        {
            return Start.Equals(other.Start) && End.Equals(other.End);
        }

        public bool Equals(DateTime other)
        {
            return Equals(Period.Instant(other));
        }

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case ConcretePeriod other when Equals(other):
                case Period period when Equals(period):
                case DateTime moment when Equals(moment):
                    return true;
                default:
                    return false;
            }
        }

        public static implicit operator Period(ConcretePeriod period)
        {
            return new Period(period.Start, period.End);
        }
    }
}
