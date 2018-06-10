using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MiningMonitor.Data.Repository;
using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public class SnapshotService : ISnapshotService
    {
        private readonly ISnapshotRepository _repository;

        public SnapshotService(ISnapshotRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Snapshot>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<Snapshot>> GetByMinerAsync(Guid minerId, DateTime? from, DateTime? to, TimeSpan interval)
        {
            var end = to ?? DateTime.Now;
            var start = from ?? end.AddMinutes(-60);
            var snapshots = await _repository.GetByMinerAsync(minerId, start, end);

            return snapshots.FillGaps(start, end, interval).ToList();
        }

        public async Task AddAsync(Snapshot snapshot)
        {
            snapshot.Id = Guid.NewGuid();

            await _repository.AddAsync(snapshot);
        }

        public async Task UpsertAsync(Snapshot snapshot)
        {
            if (await _repository.GetByIdAsync(snapshot.Id) != null)
                await _repository.UpdateAsync(snapshot);
            else
                await _repository.AddAsync(snapshot);
        }

        public async Task DeleteAsync(Guid snapshotId)
        {
            await _repository.DeleteAsync(snapshotId);
        }

        public async Task DeleteByMinerAsync(Guid minerId)
        {
            await _repository.DeleteAsync(s => s.MinerId == minerId);
        }

        public async Task<int> DeleteOldAsync(DateTime cutoff)
        {
            return await _repository.DeleteAsync(s => s.SnapshotTime < cutoff);
        }
    }
}