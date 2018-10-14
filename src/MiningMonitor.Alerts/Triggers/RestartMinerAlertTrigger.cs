using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;
using MiningMonitor.Service;

namespace MiningMonitor.Alerts.Triggers
{
    public class RestartMinerAlertTrigger : IAlertTrigger
    {
        private readonly IRemoteManagementClientFactory _clientFactory;

        public RestartMinerAlertTrigger(IRemoteManagementClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public bool ShouldTrigger(AlertTriggerDefinition triggerDefinition, Miner miner)
        {
            return miner.CollectorId == null && triggerDefinition.Type == TriggerType.RestartMiner;
        }

        public async Task<TriggerResult> ExecuteTriggerAsync(AlertTriggerDefinition triggerDefinition, AlertDefinition alertDefinition, Miner miner, Alert alert, CancellationToken token)
        {
            var client = _clientFactory.Create(miner);
            
            await client.RestartMinerAsync();
            return TriggerResult.Complete($"Restarting miner {miner.Name}", triggerDefinition.Name);
        }
    }
}