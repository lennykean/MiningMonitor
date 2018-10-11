using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public interface ICollectorService
    {
        Task<IEnumerable<Collector>> GetAllAsync(CancellationToken token = default);
        Task<(bool success, Collector collector)> GetAsync(string collectorId, CancellationToken token = default);
        Task<(ModelStateDictionary modelState, RegistrationResponse registration)> CreateCollectorAsync(Collector collector, CancellationToken token = default);
        Task<bool> UpdateAsync(Collector collector, CancellationToken token = default);
        Task<bool> DeleteAsync(string collectorId, CancellationToken token = default);
        Task<bool> MinerSyncAsync(string collector, Miner miner, CancellationToken token = default);
        Task<bool> SnapshotSyncAsync(string collector, Guid minerId, Snapshot snapshot, CancellationToken token = default);
    }
}