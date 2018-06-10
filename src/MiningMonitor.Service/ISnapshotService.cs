using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public interface ISnapshotService
    {
        Task<IEnumerable<Snapshot>> GetAllAsync();
        Task<IEnumerable<Snapshot>> GetByMinerAsync(Guid minerId, DateTime? from, DateTime? to, TimeSpan interval);
        Task AddAsync(Snapshot snapshot);
        Task UpsertAsync(Snapshot snapshot);
        Task DeleteAsync(Guid snapshotId);
        Task DeleteByMinerAsync(Guid minerId);
        Task<int> DeleteOldAsync(DateTime cutoff);
    }
}