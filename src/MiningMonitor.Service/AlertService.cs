using System;
using System.Collections.Generic;

using LiteDB;

using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service
{
    public class AlertService : IAlertService
    {
        private readonly LiteCollection<AlertDefinition> _alertDefinitionCollection;
        private readonly LiteCollection<Alert> _alertCollection;

        public AlertService(LiteCollection<AlertDefinition> alertDefinitionCollection, LiteCollection<Alert> alertCollection)
        {
            _alertDefinitionCollection = alertDefinitionCollection;
            _alertCollection = alertCollection;

            _alertCollection.EnsureIndex(a => a.MinerId);
            _alertCollection.EnsureIndex(a => a.AlertDefinitionId);
        }

        public IEnumerable<AlertDefinition> GetDefinitions()
        {
            return _alertDefinitionCollection.FindAll();
        }

        public IEnumerable<AlertDefinition> GetDefinitionsByMiner(Guid minerId)
        {
            return _alertDefinitionCollection.Find(a => a.MinerId == minerId);
        }

        public IEnumerable<Alert> GetActiveAlertsByMiner(Guid minerId)
        {
            return _alertCollection.Find(a => !a.Acknowledged && a.MinerId == minerId);
        }

        public AlertDefinition GetDefinition(Guid id)
        {
            return _alertDefinitionCollection.FindById(id);
        }

        public void Add(AlertDefinition alertDefinition)
        {
            alertDefinition.Id = Guid.NewGuid();
            alertDefinition.Created = DateTime.UtcNow;

            if (alertDefinition.Enabled)
                alertDefinition.LastEnabled = DateTime.UtcNow;

            _alertDefinitionCollection.Insert(alertDefinition);
        }

        public bool Update(AlertDefinition alertDefinition)
        {
            var current = _alertDefinitionCollection.FindById(alertDefinition.Id);
            if (current == null)
                return false;

            alertDefinition.Updated = DateTime.UtcNow;

            if (!current.Enabled && alertDefinition.Enabled)
                alertDefinition.LastEnabled = DateTime.UtcNow;
            
            return _alertDefinitionCollection.Update(alertDefinition);
        }

        public bool DeleteDefinition(Guid id)
        {
            return _alertDefinitionCollection.Delete(id);
        }

        public bool AcknowledgeAlert(Guid alertId)
        {
            var alert = _alertCollection.FindById(alertId);
            if (alert == null)
                return false;

            alert.Acknowledged = true;
            alert.AcknowledgedAt = DateTime.UtcNow;

            return _alertCollection.Update(alert);
        }
    }
}
