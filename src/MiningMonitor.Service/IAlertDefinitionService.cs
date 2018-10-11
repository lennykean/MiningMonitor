using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service
{
    public interface IAlertDefinitionService
    {
        Task<IEnumerable<AlertDefinition>> GetAllAsync(CancellationToken token = default);
        Task<IEnumerable<AlertDefinition>> GetEnabledAsync(CancellationToken token = default);
        Task<IEnumerable<AlertDefinition>> GetByMinerAsync(Guid minerId, CancellationToken token = default);
        Task<AlertDefinition> GetByIdAsync(Guid id, CancellationToken token = default);
        Task AddAsync(AlertDefinition alertDefinition, CancellationToken token = default);
        Task<bool> UpdateAsync(AlertDefinition alertDefinition, CancellationToken token = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken token = default);
        Task<bool> MarkScannedAsync(Guid id, DateTime scanTime, CancellationToken token = default);
    }
}