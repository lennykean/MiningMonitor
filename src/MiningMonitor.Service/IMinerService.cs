using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public interface IMinerService
    {
        Task<IEnumerable<Miner>> GetEnabledMinersAsync();
        Task<IEnumerable<Miner>> GetAllAsync();
        Task<Miner> GetByIdAsync(Guid id);
        Task AddAsync(Miner miner);
        Task AddExistingAsync(Miner miner);
        Task<bool> UpdateAsync(Miner miner);
        Task<bool> SetSyncedAsync(Miner miner);
        Task<bool> DeleteAsync(Guid id);
        Task DeleteByCollectorAsync(string collectorId);
    }
}
