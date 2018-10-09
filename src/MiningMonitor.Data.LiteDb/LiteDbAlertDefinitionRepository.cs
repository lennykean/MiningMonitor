using LiteDB;

using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Data.LiteDb
{
    public class LiteDbAlertDefinitionRepository : LiteDbRepository<AlertDefinition>
    {
        public LiteDbAlertDefinitionRepository(LiteCollection<AlertDefinition> collection) : base(collection)
        {
            collection.EnsureIndex(a => a.MinerId);
        }
    }
}
