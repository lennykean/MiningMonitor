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
        }

        public Task<IEnumerable<Snapshot>> GetByMinerAsync(Guid minerId, DateTime? from, DateTime? to)
        {
            return Task.Run(() => DbCollection.Find(snapshot => minerId == snapshot.MinerId && (from == null || from <= snapshot.SnapshotTime) && (to == null || to >= snapshot.SnapshotTime)));
        }

        public Task<int> DeleteByMinerAsync(Guid minerId)
        {
            return Task.Run(() => DbCollection.Delete(snapshot => snapshot.MinerId == minerId));
        }
    }
}