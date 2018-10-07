using System;
using System.Collections.Generic;
using System.Linq;

namespace MiningMonitor.Model
{
    public struct Period
    {
        public override int GetHashCode()
        {
            unchecked
            {
                return (Start.GetHashCode() * 397) ^ End.GetHashCode();
            }
        }

        public Period(DateTime? start, DateTime? end)
        {
            if (end < start)
                throw new ArgumentException("end cannot be before start", nameof(end));

            Start = start;
            End = end;
        }
        
        public DateTime? Start { get; }
        public DateTime? End { get; }
        public TimeSpan? Duration => End - Start;
        public bool HasStart => Start.HasValue;
        public bool HasEnd => End.HasValue;
        public bool IsInfinite => !HasEnd && !HasStart;
        public bool IsInstant => !IsInfinite && Start == End;

        public Period Infinity => new Period();

        public bool Contains(Period other)
        {
            if (HasStart && HasEnd && other.HasStart && other.HasEnd)
                return other.Start >= Start && other.End <= End;
            if (HasStart && other.HasStart)
                return other.Start >= Start;
            if (HasStart)
                return false;
            if (HasEnd && other.HasEnd)
                return other.Start >= Start;
            if (HasEnd)
                return false;

            return true;
        }

        public bool Contains(DateTime? moment)
        {
            if (moment == null)
                return false;

            return Contains(Instant((DateTime)moment));
        }

        public static ConcretePeriod Instant(DateTime moment)
        {
            return new ConcretePeriod(moment, moment);
        }

        public bool Overlaps(Period other)
        {
            return Contains(new Period(other.Start, other.Start)) || Contains(new Period(other.End, other.End));
        }
        
        public bool Equals(Period other)
        {
            return Start.Equals(other.Start) && End.Equals(other.End);
        }

        public bool Equals(DateTime? other)
        {
            if (other == null)
                return false;

            return Equals(Instant((DateTime)other));
        }

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case Period other when Equals(other):
                case DateTime moment when Equals(moment):
                    return true;
                default:
                    return false;
            }
        }
        
        public static explicit operator ConcretePeriod(Period period)
        {
            if (period.Start == null || period.End == null)
                throw new InvalidCastException();

            return new ConcretePeriod((DateTime)period.Start, (DateTime)period.End);
        }

        public static bool operator ==(Period period, Period other)
        {
            return period.Equals(other);
        }

        public static bool operator !=(Period period, Period other)
        {
            return !period.Equals(other);
        }

        public static bool operator <(Period period, Period other)
        {
            return period.End < other.Start;
        }

        public static bool operator >(Period period, Period other)
        {
            return period.Start > other.End;
        }
        
        public static bool operator ==(Period period, DateTime? other)
        {
            return period.Equals(other);
        }

        public static bool operator !=(Period period, DateTime? other)
        {
            return !period.Equals(other);
        }

        public static bool operator <(Period period, DateTime? other)
        {
            if (other == null)
                return false;

            return period < Instant((DateTime)other);
        }

        public static bool operator >(Period period, DateTime? other)
        {
            if (other == null)
                return false;

            return period > Instant((DateTime)other);
        }

        public static Period Merge(IEnumerable<Period> periods)
        {
            return (
                from period in periods
                group period by 0 into g
                let start = g.Any(period => !period.HasStart) ? null : g.Min(p => p.Start)
                let end = g.Any(period => !period.HasEnd) ? null : g.Max(p => p.End)
                select new Period(start, end)).Single();
        }
    }
}
