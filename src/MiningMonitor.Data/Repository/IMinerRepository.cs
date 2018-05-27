using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MiningMonitor.Model;

namespace MiningMonitor.Data.Repository
{
    public interface IMinerRepository : IRepository<Miner, Guid>
    {
        Task<IEnumerable<Miner>> GetEnabledMinersAsync();
    }
}