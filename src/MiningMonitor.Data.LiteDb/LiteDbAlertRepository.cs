using System;

using LiteDB;

using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Data.LiteDb
{
    public class LiteDbAlertRepository : LiteDbRepository<Alert, Guid>
    {
        public LiteDbAlertRepository(LiteCollection<Alert> collection) : base(collection)
        {
            collection.EnsureIndex(a => a.MinerId);
            collection.EnsureIndex(a => a.AlertDefinitionId);
        }
    }
}
