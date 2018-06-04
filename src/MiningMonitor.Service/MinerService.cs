using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MiningMonitor.Data.Repository;
using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public class MinerService : IMinerService
    {
        private readonly IMinerRepository _minerRepo;
        private readonly ISnapshotService _snapshotService;

        public MinerService(IMinerRepository minerRepo, ISnapshotService snapshotService)
        {
            _minerRepo = minerRepo;
            _snapshotService = snapshotService;
        }

        public async Task<IEnumerable<Miner>> GetEnabledMinersAsync()
        {
            return await _minerRepo.GetEnabledMinersAsync();
        }

        public async Task<IEnumerable<Miner>> GetAllAsync()
        {
            return await _minerRepo.GetAllAsync();
        }

        public async Task<Miner> GetByIdAsync(Guid id)
        {
            return await _minerRepo.GetByIdAsync(id);
        }

        public async Task AddAsync(Miner miner)
        {
            miner.Id = Guid.NewGuid();
            miner.IsSynced = false;

            await _minerRepo.AddAsync(miner);
        }

        public async Task<bool> UpdateAsync(Miner miner)
        {
            if (miner.CollectorId != null)
                return false;

            miner.IsSynced = false;
            return await _minerRepo.UpdateAsync(miner);
        }
        public async Task<bool> SetSyncedAsync(Miner miner)
        {
            miner.IsSynced = true;
            return await _minerRepo.UpdateAsync(miner);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _snapshotService.DeleteByMinerAsync(id);

            return await _minerRepo.DeleteAsync(id);
        }

        public async Task DeleteByCollectorAsync(string collectorId)
        {
            foreach (var minerId in (
                from miner in await _minerRepo.GetAllAsync()
                where miner.CollectorId == collectorId
                select miner.Id).ToList())
            {
                await _snapshotService.DeleteByMinerAsync(minerId);
                await _minerRepo.DeleteAsync(minerId);
            }
        }
    }
}
