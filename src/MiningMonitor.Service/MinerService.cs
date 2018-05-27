using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MiningMonitor.Data.Repository;
using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public class MinerService : IMinerService
    {
        private readonly IMinerRepository _minerRepo;

        public MinerService(IMinerRepository minerRepo)
        {
            _minerRepo = minerRepo;
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
            return await _minerRepo.DeleteAsync(id);
        }

        public async Task<bool> CollectorSyncAsync(string collector, Miner miner)
        {
            var existing = await _minerRepo.GetByIdAsync(miner.Id);
            if (existing != null)
            {
                if (existing.CollectorId != collector)
                    return false;

                miner.CollectorId = collector;
                await _minerRepo.UpdateAsync(miner);
            }
            else
            {
                miner.CollectorId = collector;
                await _minerRepo.AddAsync(miner);
            }
            return true;
        }
    }
}
