using System;
using System.Collections.Generic;

using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service.Alerts
{
    public interface IAlertDefinitionService
    {
        IEnumerable<AlertDefinition> GetAll();
        IEnumerable<AlertDefinition> GetEnabled();
        IEnumerable<AlertDefinition> GetByMiner(Guid minerId);
        AlertDefinition GetById(Guid id);
        void Add(AlertDefinition alertDefinition);
        bool Update(AlertDefinition alertDefinition);
        void MarkScanned(AlertDefinition alertDefinition, DateTime scanTime);
        bool Delete(Guid id);
    }
}