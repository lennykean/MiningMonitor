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
        private readonly IRepository<Alert, Guid> _alertRepository;

        public AlertService(IRepository<Alert, Guid> alertRepository)
        {
            _alertRepository = alertRepository;
        }

        public async Task<IEnumerable<Alert>> GetAsync(bool includeAcknowledged, CancellationToken token = default)
        {
            return (await _alertRepository.FindAsync(a => includeAcknowledged || a.AcknowledgedAt == null, token))
                .OrderByDescending(a => a.Active)
                .ThenByDescending(a => a.Severity)
                .ThenBy(a => a.Start);
        }
        
        public async Task<IEnumerable<Alert>> GetByMinerAsync(Guid minerId, bool includeAcknowledged, CancellationToken token = default)
        {
            return (await _alertRepository.FindAsync(a => a.MinerId == minerId && (includeAcknowledged || a.AcknowledgedAt == null), token))
                .OrderBy(a => a.Start);
        }
        
        public async Task<IEnumerable<Alert>> GetActiveByDefinitionAsync(Guid definitionId, DateTime? since = null, CancellationToken token = default)
        {
            return await _alertRepository.FindAsync(a => a.AlertDefinitionId == definitionId && a.End == null && (since == null || since < a.LastActive), token);
        }

        public async Task<Alert> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return await _alertRepository.FindByIdAsync(id, token);
        }

        public async Task AddAsync(Alert alert, CancellationToken token = default)
        {
            alert.Id = Guid.NewGuid();

            await _alertRepository.InsertAsync(alert, token);
        }

        public async Task<bool> UpdateAsync(Alert alert, CancellationToken token = default)
        {
            return await _alertRepository.UpdateAsync(alert, token);
        }
        
        public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            return await _alertRepository.DeleteAsync(id, token);
        }

        public async Task<bool> AcknowledgeAsync(Guid alertId, CancellationToken token = default)
        {
            var alert = await _alertRepository.FindByIdAsync(alertId, token);
            if (alert == null)
                return false;

            alert.AcknowledgedAt = DateTime.UtcNow;

            return await _alertRepository.UpdateAsync(alert, token);
        }

        public async Task<int> DeleteOldAsync(DateTime cutoff, CancellationToken token)
        {
            return await _alertRepository.DeleteAsync(s => s.Acknowledged && s.Start < cutoff, token);
        }
    }
}
