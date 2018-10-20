using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service
{
    public interface IAlertService
    {
        Task<IEnumerable<Alert>> GetAsync(bool includeAcknowledged, CancellationToken token = default);
        Task<IEnumerable<Alert>> GetByMinerAsync(Guid minerId, bool includeAcknowledged, CancellationToken token = default);
        Task<IEnumerable<Alert>> GetActiveByDefinitionAsync(Guid definitionId, DateTime? since = null, CancellationToken token = default);
        Task<Alert> GetByIdAsync(Guid id, CancellationToken token = default);
        Task AddAsync(Alert alert, CancellationToken token = default);
        Task<bool> UpdateAsync(Alert alert, CancellationToken token = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken token = default);
        Task<bool> AcknowledgeAsync(Guid alertId, CancellationToken token = default);
        Task<int> DeleteOldAsync(DateTime purgeCutoff, CancellationToken cancellationToken);
    }
}