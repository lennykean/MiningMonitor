using System;
using System.Collections.Generic;
using System.Linq;

using LiteDB;

using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public class MinerService : IMinerService
    {
        private readonly LiteCollection<Miner> _minerCollection;
        private readonly ISnapshotService _snapshotService;

        public MinerService(LiteCollection<Miner> minerCollection, ISnapshotService snapshotService)
        {
            _minerCollection = minerCollection;
            _snapshotService = snapshotService;
        }

        public IEnumerable<Miner> GetEnabledMiners()
        {
            return _minerCollection.Find(miner => miner.CollectData && miner.CollectorId == null);
        }

        public IEnumerable<Miner> GetAll()
        {
            return _minerCollection.FindAll();
        }

        public Miner GetById(Guid id)
        {
            return _minerCollection.FindById(id);
        }

        public void Add(Miner miner)
        {
            miner.Id = Guid.NewGuid();
            miner.IsSynced = false;

            _minerCollection.Insert(miner);
        }

        public bool Update(Miner miner)
        {
            if (miner.CollectorId != null)
                return false;

            miner.IsSynced = false;

            return _minerCollection.Update(miner);
        }

        public void Upsert(Miner miner)
        {
            _minerCollection.Upsert(miner);
        }

        public bool SetSynced(Miner miner)
        {
            miner.IsSynced = true;
            return _minerCollection.Update(miner);
        }

        public bool Delete(Guid id)
        {
            _snapshotService.DeleteByMiner(id);

            return _minerCollection.Delete(id);
        }

        public void DeleteByCollector(string collectorId)
        {
            foreach (var minerId in (
                from miner in _minerCollection.FindAll()
                where miner.CollectorId == collectorId
                select miner.Id).ToList())
            {
                _snapshotService.DeleteByMiner(minerId);
                _minerCollection.Delete(minerId);
            }
        }
    }
}
