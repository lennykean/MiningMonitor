using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LiteDB;

using MiningMonitor.Model;

namespace MiningMonitor.Data.Repository
{
    public class SnapshotRepository : LiteDbRepository<Snapshot, Guid>, ISnapshotRepository
    {
        public SnapshotRepository(LiteCollection<Snapshot> dbCollection) : base(dbCollection)
        {
            dbCollection.EnsureIndex(s => s.MinerId);
            dbCollection.EnsureIndex(s => s.SnapshotTime);
        }

        public Task<IEnumerable<Snapshot>> GetByMinerAsync(Guid minerId, DateTime? from, DateTime? to)
        {
            return Task.Run(() => DbCollection.Find(snapshot => snapshot.MinerId == minerId && (from == null || snapshot.SnapshotTime >= from) && (to == null || snapshot.SnapshotTime < to)));
        }

        public Task<int> DeleteByMinerAsync(Guid minerId)
        {
            return Task.Run(() => DbCollection.Delete(snapshot => snapshot.MinerId == minerId));
        }
    }
}