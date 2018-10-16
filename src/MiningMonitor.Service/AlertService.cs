using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Data;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service
{
    public class AlertService : IAlertService
    {
        private readonly IRepository<Alert> _alertCollection;

        public AlertService(IRepository<Alert> alertCollection)
        {
            _alertCollection = alertCollection;
        }

        public async Task<IEnumerable<Alert>> GetAsync(bool includeAcknowledged, CancellationToken token = default)
        {
            return (await _alertCollection.FindAsync(a => includeAcknowledged || a.AcknowledgedAt == null, token))
                .OrderByDescending(a => a.Active)
                .ThenByDescending(a => a.Severity)
                .ThenBy(a => a.Start);
        }
        
        public async Task<IEnumerable<Alert>> GetByMinerAsync(Guid minerId, bool includeAcknowledged, CancellationToken token = default)
        {
            return (await _alertCollection.FindAsync(a => a.MinerId == minerId && (includeAcknowledged || a.AcknowledgedAt == null), token))
                .OrderBy(a => a.Start);
        }
        
        public async Task<IEnumerable<Alert>> GetActiveByDefinitionAsync(Guid definitionId, DateTime? since = null, CancellationToken token = default)
        {
            return await _alertCollection.FindAsync(a => a.AlertDefinitionId == definitionId && a.End == null && (since == null || since < a.LastActive), token);
        }

        public async Task<Alert> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return await _alertCollection.FindByIdAsync(id, token);
        }

        public async Task AddAsync(Alert alert, CancellationToken token = default)
        {
            alert.Id = Guid.NewGuid();

            await _alertCollection.InsertAsync(alert, token);
        }

        public async Task<bool> UpdateAsync(Alert alert, CancellationToken token = default)
        {
            return await _alertCollection.UpdateAsync(alert, token);
        }
        
        public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            return await _alertCollection.DeleteAsync(id, token);
        }

        public async Task<bool> AcknowledgeAsync(Guid alertId, CancellationToken token = default)
        {
            var alert = await _alertCollection.FindByIdAsync(alertId, token);
            if (alert == null)
                return false;

            alert.AcknowledgedAt = DateTime.UtcNow;

            return await _alertCollection.UpdateAsync(alert, token);
        }
    }
}
