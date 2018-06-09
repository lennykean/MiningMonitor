using ClaymoreMiner.RemoteManagement;

using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public interface IRemoteManagementClientFactory
    {
        IRemoteManagementClient Create(Miner miner);
    }
}