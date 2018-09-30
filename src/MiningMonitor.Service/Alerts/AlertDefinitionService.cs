using System;
using System.Collections.Generic;

using LiteDB;

using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service.Alerts
{
    public class AlertDefinitionService : IAlertDefinitionService
    {
        private readonly LiteCollection<AlertDefinition> _alertDefinitionCollection;

        public AlertDefinitionService(LiteCollection<AlertDefinition> alertDefinitionCollection)
        {
            _alertDefinitionCollection = alertDefinitionCollection;
        }

        public IEnumerable<AlertDefinition> GetAll()
        {
            return _alertDefinitionCollection.FindAll();
        }

        public IEnumerable<AlertDefinition> GetEnabled()
        {
            return _alertDefinitionCollection.Find(a => a.Enabled);
        }

        public IEnumerable<AlertDefinition> GetByMiner(Guid minerId)
        {
            return _alertDefinitionCollection.Find(a => a.MinerId == minerId);
        }

        public AlertDefinition GetById(Guid id)
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

            alertDefinition.MinerId = current.MinerId;
            alertDefinition.Created = current.Created;
            alertDefinition.LastScan = current.LastScan;
            alertDefinition.Updated = DateTime.UtcNow;
            if (alertDefinition.Parameters.AlertType != current.Parameters.AlertType)
                alertDefinition.Parameters = current.Parameters;

            if (!current.Enabled && alertDefinition.Enabled)
                alertDefinition.LastEnabled = DateTime.UtcNow;
            else
                alertDefinition.LastEnabled = current.LastEnabled;
            
            return _alertDefinitionCollection.Update(alertDefinition);
        }

        public bool Delete(Guid id)
        {
            return _alertDefinitionCollection.Delete(id);
        }

        public bool MarkScanned(Guid id, DateTime scanTime)
        {
            var alertDefinition = _alertDefinitionCollection.FindById(id);
            if (alertDefinition == null)
                return false;

            alertDefinition.LastScan = scanTime;
            _alertDefinitionCollection.Update(alertDefinition);
            return true;
        }
    }
}
