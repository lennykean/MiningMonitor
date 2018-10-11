using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public interface IServerService
    {
        Task<(string id, string token)> RegisterAsCollectorAsync(CancellationToken token = default);
        Task<bool> CheckApprovalAsync(string id, CancellationToken token = default);
        Task SyncMinerAsync(string id, Miner miner, CancellationToken token = default);
        Task SyncSnapshotAsync(string id, Snapshot snapshot, CancellationToken token = default);
    }
}
