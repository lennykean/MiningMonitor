using System;
using System.Collections.Generic;

using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public interface IMinerService
    {
        IEnumerable<Miner> GetEnabledMiners();
        IEnumerable<Miner> GetAll();
        Miner GetById(Guid id);
        void Add(Miner miner);
        bool Update(Miner miner);
        void Upsert(Miner miner);
        bool SetSynced(Miner miner);
        bool Delete(Guid id);
        void DeleteByCollector(string collectorId);
    }
}
