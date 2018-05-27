using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MiningMonitor.Model;

namespace MiningMonitor.Data.Repository
{
    public interface ISnapshotRepository : IRepository<Snapshot, Guid>
    {
        Task<IEnumerable<Snapshot>> GetByMinerAsync(Guid minerId, DateTime? from, DateTime? to);
    }
}