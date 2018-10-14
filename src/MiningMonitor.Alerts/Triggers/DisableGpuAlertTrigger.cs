using System.Threading;
using System.Threading.Tasks;

using ClaymoreMiner.RemoteManagement.Models;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;
using MiningMonitor.Service;

namespace MiningMonitor.Alerts.Triggers
{
    public class DisableGpuAlertTrigger : IAlertTrigger
    {
        private readonly IRemoteManagementClientFactory _clientFactory;

        public DisableGpuAlertTrigger(IRemoteManagementClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public bool ShouldTrigger(AlertTriggerDefinition triggerDefinition, Miner miner)
        {
            return miner.CollectorId == null && triggerDefinition.Type == TriggerType.DisableGpu;
        }

        public async Task<TriggerResult> ExecuteTriggerAsync(AlertTriggerDefinition triggerDefinition, AlertDefinition alertDefinition, Miner miner, Alert alert, CancellationToken token)
        {
            var trigger = (DisableGpuAlertTriggerDefinition)triggerDefinition;
            var client = _clientFactory.Create(miner);

            if (trigger.DisableAll)
            {
                await client.SetGpuModeAsync(GpuMode.Disabled);
                return TriggerResult.Complete($"Sent control message to disabled GPUs on miner {miner.Name}", trigger.Name);
            }
            if (trigger.DisableAffected && alert.Metadata?.GpuIndex != null)
            {
                await client.SetGpuModeAsync((int)alert.Metadata.GpuIndex, GpuMode.Disabled);
                return TriggerResult.Complete($"Sent control message to disable GPU {alert.Metadata.GpuIndex + 1} on miner {miner.Name}", trigger.Name);
            }
            return TriggerResult.Skip("Unable to determine which GPU to disable", trigger.Name);
        }
    }
}
