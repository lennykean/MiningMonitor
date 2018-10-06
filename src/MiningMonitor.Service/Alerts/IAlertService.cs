using System;
using System.Collections.Generic;

using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service.Alerts
{
    public interface IAlertService
    {
        IEnumerable<Alert> Get(bool includeAcknowledged);
        IEnumerable<Alert> GetByMiner(Guid minerId, bool includeAcknowledged);
        IEnumerable<Alert> GetActiveByDefinition(Guid definitionId, DateTime? since = null);
        Alert GetById(Guid id);
        void Add(Alert alert);
        bool Update(Alert alert);
        bool Delete(Guid id);
        bool Acknowledge(Guid alertId);
    }
}