using System;
using System.Collections.Generic;

using MiningMonitor.Common;
using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public interface ISnapshotService
    {
        IEnumerable<Snapshot> GetAll();
        IEnumerable<Snapshot> GetByMiner(Guid minerId, Period period);
        IEnumerable<Snapshot> GetByMinerFillGaps(Guid minerId, ConcretePeriod period, TimeSpan interval);
        void Add(Snapshot snapshot);
        void Upsert(Snapshot snapshot);
        void Delete(Guid snapshotId);
        void DeleteByMiner(Guid minerId);
        int DeleteOld(DateTime cutoff);
    }
}