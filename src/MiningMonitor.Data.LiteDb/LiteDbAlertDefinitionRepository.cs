using System;

using LiteDB;

using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Data.LiteDb
{
    public class LiteDbAlertDefinitionRepository : LiteDbRepository<AlertDefinition, Guid>
    {
        public LiteDbAlertDefinitionRepository(LiteCollection<AlertDefinition> collection) : base(collection)
        {
            collection.EnsureIndex(a => a.MinerId);
        }
    }
}
