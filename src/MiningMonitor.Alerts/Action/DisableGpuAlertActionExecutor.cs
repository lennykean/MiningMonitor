using System.Threading;
using System.Threading.Tasks;

using ClaymoreMiner.RemoteManagement.Models;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;
using MiningMonitor.Model.Alerts.Actions;
using MiningMonitor.Service;

namespace MiningMonitor.Alerts.Action
{
    public class DisableGpuAlertActionExecutor : IAlertActionExecutor
    {
        private readonly IRemoteManagementClientFactory _clientFactory;

        public DisableGpuAlertActionExecutor(IRemoteManagementClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public bool ShouldExecute(AlertActionDefinition actionDefinition, Miner miner)
        {
            return miner.CollectorId == null && actionDefinition.Type == AlertActionType.DisableGpu;
        }

        public async Task<AlertActionResult> ExecuteActionAsync(AlertActionDefinition actionDefinition, AlertDefinition alertDefinition, Miner miner, AlertMetadata metadata, CancellationToken token)
        {
            var action = (DisableGpuAlertActionDefinition)actionDefinition;
            var client = _clientFactory.Create(miner);

            if (action.DisableAll)
            {
                await client.SetGpuModeAsync(GpuMode.Disabled);
                return AlertActionResult.Complete($"Sent control message to disabled GPUs on miner {miner.Name}", action.Name);
            }
            if (action.DisableAffected && metadata?.GpuIndex != null)
            {
                await client.SetGpuModeAsync((int)metadata.GpuIndex, GpuMode.Disabled);
                return AlertActionResult.Complete($"Sent control message to disable GPU {metadata.GpuIndex + 1} on miner {miner.Name}", action.Name);
            }
            return AlertActionResult.Skip("Unable to determine which GPU to disable", action.Name);
        }
    }
}
