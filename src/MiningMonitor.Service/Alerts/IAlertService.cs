using System;
using System.Collections.Generic;

using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service.Alerts
{
    public interface IAlertService
    {
        IEnumerable<Alert> Get(bool includeAcknowledged);
        IEnumerable<Alert> GetByMiner(Guid minerId, bool includeAcknowledged);
        Alert GetById(Guid id);
        Alert GetLatestActiveByDefinition(Guid definitionId, DateTime? since = null);
        void Add(Alert alert);
        bool Acknowledge(Guid alertId);
        bool End(Guid alertId);
        bool Delete(Guid id);
    }
}