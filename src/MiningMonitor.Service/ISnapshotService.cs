using System;
using System.Collections.Generic;

using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public interface ISnapshotService
    {
        IEnumerable<Snapshot> GetAll();
        IEnumerable<Snapshot> GetByMiner(Guid minerId, DateTime? from, DateTime? to, TimeSpan interval);
        void Add(Snapshot snapshot);
        void Upsert(Snapshot snapshot);
        void Delete(Guid snapshotId);
        void DeleteByMiner(Guid minerId);
        int DeleteOld(DateTime cutoff);
    }
}