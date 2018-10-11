using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Common;
using MiningMonitor.Data;
using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public class SnapshotService : ISnapshotService
    {
        private readonly IRepository<Snapshot> _collection;

        public SnapshotService(IRepository<Snapshot> collection)
        {
            _collection = collection;
        }

        public async Task<IEnumerable<Snapshot>> GetAllAsync(CancellationToken token = default)
        {
            return await _collection.FindAllAsync(token);
        }

        public async Task<IEnumerable<Snapshot>> GetByMinerAsync(Guid minerId, Period period, CancellationToken token = default)
        {
            return await _collection.FindAsync(snapshot => snapshot.MinerId == minerId && snapshot.SnapshotTime >= period.Start && snapshot.SnapshotTime <= period.End, token);
        }

        public async Task<IEnumerable<Snapshot>> GetByMinerFillGapsAsync(Guid minerId, ConcretePeriod period, TimeSpan interval, CancellationToken token = default)
        {
            return (await GetByMinerAsync(minerId, period, token)).FillGaps(period, interval).ToList();
        }

        public async Task AddAsync(Snapshot snapshot, CancellationToken token = default)
        {
            snapshot.Id = Guid.NewGuid();

            await _collection.InsertAsync(snapshot, token);
        }

        public async Task UpsertAsync(Snapshot snapshot, CancellationToken token = default)
        {
            await _collection.UpsertAsync(snapshot, token);
        }

        public async Task DeleteAsync(Guid snapshotId, CancellationToken token = default)
        {
            await _collection.DeleteAsync(snapshotId, token);
        }

        public async Task DeleteByMinerAsync(Guid minerId, CancellationToken token = default)
        {
            await _collection.DeleteAsync(s => s.MinerId == minerId, token);
        }

        public async Task<int> DeleteOldAsync(DateTime cutoff, CancellationToken token = default)
        {
            return await _collection.DeleteAsync(s => s.SnapshotTime < cutoff, token);
        }
    }
}