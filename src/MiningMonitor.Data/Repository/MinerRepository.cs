using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LiteDB;

using MiningMonitor.Model;

namespace MiningMonitor.Data.Repository
{
    public class MinerRepository : LiteDbRepository<Miner, Guid>, IMinerRepository
    {
        public MinerRepository(LiteCollection<Miner> dbCollection) : base(dbCollection)
        {
        }

        public Task<IEnumerable<Miner>> GetEnabledMinersAsync()
        {
            return Task.Run(() => DbCollection.Find(miner => miner.CollectData && miner.CollectorId == null));
        }
    }
}