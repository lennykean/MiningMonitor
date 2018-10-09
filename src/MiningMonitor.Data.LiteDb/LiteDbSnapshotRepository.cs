using LiteDB;

using MiningMonitor.Model;

namespace MiningMonitor.Data.LiteDb
{
    public class LiteDbSnapshotRepository : LiteDbRepository<Snapshot>
    {
        public LiteDbSnapshotRepository(LiteCollection<Snapshot> collection) : base(collection)
        {
            collection.EnsureIndex(s => s.MinerId);
            collection.EnsureIndex(s => s.SnapshotTime);
        }
    }
}
