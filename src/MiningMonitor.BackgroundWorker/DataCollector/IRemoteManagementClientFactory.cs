using ClaymoreMiner.RemoteManagement;

using MiningMonitor.Model;

namespace MiningMonitor.BackgroundWorker.DataCollector
{
    public interface IRemoteManagementClientFactory
    {
        IRemoteManagementClient Create(Miner miner);
    }
}