using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Common;
using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public interface ISnapshotService
    {
        Task<IEnumerable<Snapshot>> GetAllAsync(CancellationToken token = default);
        Task<IEnumerable<Snapshot>> GetByMinerAsync(Guid minerId, Period period, CancellationToken token = default);
        Task<IEnumerable<Snapshot>> GetByMinerFillGapsAsync(Guid minerId, ConcretePeriod period, TimeSpan interval, CancellationToken token = default);
        Task AddAsync(Snapshot snapshot, CancellationToken token = default);
        Task UpsertAsync(Snapshot snapshot, CancellationToken token = default);
        Task DeleteAsync(Guid snapshotId, CancellationToken token = default);
        Task DeleteByMinerAsync(Guid minerId, CancellationToken token = default);
        Task<int> DeleteOldAsync(DateTime cutoff, CancellationToken token = default);
    }
}