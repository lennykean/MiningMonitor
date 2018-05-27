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
        private readonly IMinerService _minerService;

        public SnapshotService(ISnapshotRepository repository, IMinerService minerService)
        {
            _repository = repository;
            _minerService = minerService;
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

        public async Task AddAsync(IEnumerable<Snapshot> snapshots)
        {
            snapshots = snapshots.ToList();

            foreach (var snapshot in snapshots)
                snapshot.Id = Guid.NewGuid();

            await _repository.BulkAddAsync(snapshots);
        }

        public async Task DeleteAsync(Guid snapshotId)
        {
            await _repository.DeleteAsync(snapshotId);
        }

        public async Task<bool> CollectorSyncAsync(string collector, Guid minerId, Snapshot snapshot)
        {
            var miner = await _minerService.GetByIdAsync(minerId);
            if (miner?.CollectorId != collector)
                return false;

            snapshot.MinerId = minerId;
            await _repository.AddAsync(snapshot);

            return true;
        }
    }
}