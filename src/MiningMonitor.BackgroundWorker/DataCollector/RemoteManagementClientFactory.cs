using ClaymoreMiner.RemoteManagement;

using MiningMonitor.Model;

namespace MiningMonitor.BackgroundWorker.DataCollector
{
    public class RemoteManagementClientFactory : IRemoteManagementClientFactory
    {
        public IRemoteManagementClient Create(Miner miner)
        {
            return new RemoteManagementClient(miner.Address, miner.Port ?? 3333, miner.Password);
        }
    }
}