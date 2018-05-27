using System;
using System.Collections.Generic;
using System.Linq;

using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public static class SnapshotExtensions
    {
        public static IEnumerable<Snapshot> FillGaps(this IEnumerable<Snapshot> snapshots, DateTime expectedStart, DateTime expectedEnd, TimeSpan expectedInterval)
        {
            var bufferTime = expectedInterval + expectedInterval;
            var previous = new Snapshot
            {
                SnapshotTime = expectedStart
            };
            foreach (var snapshot in snapshots.OrderBy(s => s.SnapshotTime))
            {
                while (expectedInterval > TimeSpan.Zero && snapshot.SnapshotTime - previous.SnapshotTime >= bufferTime)
                {
                    previous = new Snapshot
                    {
                        SnapshotTime = previous.SnapshotTime + expectedInterval
                    };
                    yield return previous;
                }

                yield return snapshot;

                previous = snapshot;
            }

            while (expectedInterval > TimeSpan.Zero && previous.SnapshotTime <= expectedEnd - bufferTime)
            {
                previous = new Snapshot
                {
                    SnapshotTime = previous.SnapshotTime + expectedInterval
                };
                yield return previous;
            }
        }
    }
}