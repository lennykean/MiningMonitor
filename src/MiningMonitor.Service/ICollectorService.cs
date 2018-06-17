using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public interface ICollectorService
    {
        Task<IEnumerable<Collector>> GetAllAsync();
        Task<(bool success, Collector collector)> GetAsync(string collectorId);
        Task<(ModelStateDictionary modelState, RegistrationResponse registration)> CreateCollectorAsync(Collector collector);
        Task<bool> UpdateAsync(Collector collector);
        Task<bool> DeleteAsync(string collectorId);
        bool MinerSync(string collector, Miner miner);
        bool SnapshotSync(string collector, Guid minerId, Snapshot snapshot);
    }
}