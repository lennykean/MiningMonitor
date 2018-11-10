using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Data;
using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public class MinerService : IMinerService
    {
        private readonly IRepository<Miner, Guid> _minerRepository;
        private readonly ISnapshotService _snapshotService;

        public MinerService(IRepository<Miner, Guid> minerRepository, ISnapshotService snapshotService)
        {
            _minerRepository = minerRepository;
            _snapshotService = snapshotService;
        }

        public async Task<IEnumerable<Miner>> GetEnabledMinersAsync(CancellationToken token = default)
        {
            return await _minerRepository.FindAsync(miner => miner.CollectData && miner.CollectorId == null, token);
        }

        public async Task<IEnumerable<Miner>> GetAllAsync(CancellationToken token = default)
        {
            return await _minerRepository.FindAllAsync(token);
        }

        public async Task<Miner> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return await _minerRepository.FindByIdAsync(id, token);
        }

        public async Task AddAsync(Miner miner, CancellationToken token = default)
        {
            miner.Id = Guid.NewGuid();
            miner.IsSynced = false;

            await _minerRepository.InsertAsync(miner, token);
        }

        public async Task<bool> UpdateAsync(Miner miner, CancellationToken token = default)
        {
            if (miner.CollectorId != null)
                return false;

            miner.IsSynced = false;

            return await _minerRepository.UpdateAsync(miner, token);
        }

        public async Task UpsertAsync(Miner miner, CancellationToken token = default)
        {
            await _minerRepository.UpsertAsync(miner, token);
        }

        public async Task<bool> SetSyncedAsync(Miner miner, CancellationToken token = default)
        {
            miner.IsSynced = true;
            return await _minerRepository.UpdateAsync(miner, token);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            await _snapshotService.DeleteByMinerAsync(id, token);

            return await _minerRepository.DeleteAsync(id, token);
        }

        public async Task DeleteByCollectorAsync(string collectorId, CancellationToken token = default)
        {
            foreach (var minerId in (
                from miner in await _minerRepository.FindAllAsync(token)
                where miner.CollectorId == collectorId
                select miner.Id).ToList())
            {
                await _snapshotService.DeleteByMinerAsync(minerId, token);
                await _minerRepository.DeleteAsync(minerId, token);
            }
        }
    }
}
