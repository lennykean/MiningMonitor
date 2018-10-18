using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;
using MiningMonitor.Model.Alerts.Actions;
using MiningMonitor.Service;

namespace MiningMonitor.Alerts.Action
{
    public class RestartMinerAlertActionExecutor : IAlertActionExecutor
    {
        private readonly IRemoteManagementClientFactory _clientFactory;

        public RestartMinerAlertActionExecutor(IRemoteManagementClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public bool ShouldExecute(AlertActionDefinition actionDefinition, Miner miner)
        {
            return miner.CollectorId == null && actionDefinition.Type == AlertActionType.RestartMiner;
        }

        public async Task<AlertActionResult> ExecuteActionAsync(AlertActionDefinition actionDefinition, AlertDefinition alertDefinition, Miner miner, AlertMetadata metadata, CancellationToken token)
        {
            var client = _clientFactory.Create(miner);
            
            await client.RestartMinerAsync();

            return AlertActionResult.Complete(actionDefinition.Name, $"Restarting miner {miner.Name}");
        }
    }
}