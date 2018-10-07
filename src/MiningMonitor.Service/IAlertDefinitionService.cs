using System;
using System.Collections.Generic;

using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service
{
    public interface IAlertDefinitionService
    {
        IEnumerable<AlertDefinition> GetAll();
        IEnumerable<AlertDefinition> GetEnabled();
        IEnumerable<AlertDefinition> GetByMiner(Guid minerId);
        AlertDefinition GetById(Guid id);
        void Add(AlertDefinition alertDefinition);
        bool Update(AlertDefinition alertDefinition);
        bool Delete(Guid id);
        bool MarkScanned(Guid id, DateTime scanTime);
    }
}