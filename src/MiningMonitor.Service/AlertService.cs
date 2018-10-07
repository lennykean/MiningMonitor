using System;
using System.Collections.Generic;
using System.Linq;

using LiteDB;

using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service
{
    public class AlertService : IAlertService
    {
        private readonly LiteCollection<Alert> _alertCollection;

        public AlertService(LiteCollection<Alert> alertCollection)
        {
            _alertCollection = alertCollection;

            _alertCollection.EnsureIndex(a => a.MinerId);
            _alertCollection.EnsureIndex(a => a.AlertDefinitionId);
        }

        public IEnumerable<Alert> Get(bool includeAcknowledged)
        {
            return _alertCollection.Find(a => includeAcknowledged || a.AcknowledgedAt == null)
                .OrderByDescending(a => a.Active)
                .ThenByDescending(a => a.Severity)
                .ThenBy(a => a.Start);
        }
        
        public IEnumerable<Alert> GetByMiner(Guid minerId, bool includeAcknowledged)
        {
            return _alertCollection.Find(a => a.MinerId == minerId && (includeAcknowledged || a.AcknowledgedAt == null))
                .OrderBy(a => a.Start);
        }
        
        public IEnumerable<Alert> GetActiveByDefinition(Guid definitionId, DateTime? since = null)
        {
            return _alertCollection.Find(a => a.AlertDefinitionId == definitionId && a.End == null && (since == null || since < a.LastActive));
        }

        public Alert GetById(Guid id)
        {
            return _alertCollection.FindById(id);
        }

        public void Add(Alert alert)
        {
            alert.Id = Guid.NewGuid();
            alert.Start =
            alert.LastActive = DateTime.UtcNow;

            _alertCollection.Insert(alert);
        }

        public bool Update(Alert alert)
        {
            return _alertCollection.Update(alert);
        }

        public bool End(Guid alertId)
        {
            var alert = _alertCollection.FindById(alertId);
            if (alert == null)
                return false;

            alert.End = DateTime.UtcNow;

            return _alertCollection.Update(alert);
        }

        public bool Delete(Guid id)
        {
            return _alertCollection.Delete(id);
        }

        public bool Acknowledge(Guid alertId)
        {
            var alert = _alertCollection.FindById(alertId);
            if (alert == null)
                return false;

            alert.AcknowledgedAt = DateTime.UtcNow;

            return _alertCollection.Update(alert);
        }
    }
}
