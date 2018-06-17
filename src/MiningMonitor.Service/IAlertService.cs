using System;
using System.Collections.Generic;

using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public interface IAlertService
    {
        IEnumerable<AlertDefinition> GetDefinitions();
        IEnumerable<AlertDefinition> GetDefinitionsByMiner(Guid minerId);
        IEnumerable<Alert> GetActiveAlertsByMiner(Guid minerId);
        AlertDefinition GetDefinition(Guid id);
        void Add(AlertDefinition alertDefinition);
        bool Update(AlertDefinition alertDefinition);
        bool DeleteDefiniton(Guid id);
        bool AcknowledgeAlert(Guid alertId);
    }
}