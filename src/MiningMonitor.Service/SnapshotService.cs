using System;
using System.Collections.Generic;
using System.Linq;

using LiteDB;

using MiningMonitor.Common;
using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public class SnapshotService : ISnapshotService
    {
        private readonly LiteCollection<Snapshot> _collection;

        public SnapshotService(LiteCollection<Snapshot> collection)
        {
            _collection = collection;
            _collection.EnsureIndex(s => s.MinerId);
            _collection.EnsureIndex(s => s.SnapshotTime);
        }

        public IEnumerable<Snapshot> GetAll()
        {
            return _collection.FindAll();
        }

        public IEnumerable<Snapshot> GetByMiner(Guid minerId, Period period)
        {
            return _collection.Find(snapshot => snapshot.MinerId == minerId && snapshot.SnapshotTime >= period.Start && snapshot.SnapshotTime <= period.End);
        }

        public IEnumerable<Snapshot> GetByMinerFillGaps(Guid minerId, ConcretePeriod period, TimeSpan interval)
        {
            return GetByMiner(minerId, period).FillGaps(period, interval).ToList();
        }

        public void Add(Snapshot snapshot)
        {
            snapshot.Id = Guid.NewGuid();

            _collection.Insert(snapshot);
        }

        public void Upsert(Snapshot snapshot)
        {
            _collection.Upsert(snapshot);
        }

        public void Delete(Guid snapshotId)
        {
            _collection.Delete(snapshotId);
        }

        public void DeleteByMiner(Guid minerId)
        {
            _collection.Delete(s => s.MinerId == minerId);
        }

        public int DeleteOld(DateTime cutoff)
        {
            return _collection.Delete(s => s.SnapshotTime < cutoff);
        }
    }
}