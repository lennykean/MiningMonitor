using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public interface IMinerService
    {
        Task<IEnumerable<Miner>> GetEnabledMinersAsync(CancellationToken token = default);
        Task<IEnumerable<Miner>> GetAllAsync(CancellationToken token = default);
        Task<Miner> GetByIdAsync(Guid id, CancellationToken token = default);
        Task AddAsync(Miner miner, CancellationToken token = default);
        Task<bool> UpdateAsync(Miner miner, CancellationToken token = default);
        Task UpsertAsync(Miner miner, CancellationToken token = default);
        Task<bool> SetSyncedAsync(Miner miner, CancellationToken token = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken token = default);
        Task DeleteByCollectorAsync(string collectorId, CancellationToken token = default);
    }
}
