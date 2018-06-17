using System.Threading.Tasks;

using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public interface IServerService
    {
        Task<(string id, string token)> RegisterAsCollectorAsync();
        Task<bool> CheckApprovalAsync(string id);
        Task SyncMinerAsync(string id, Miner miner);
        Task SyncSnapshotAsync(string id, Snapshot snapshot);
    }
}
