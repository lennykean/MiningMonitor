using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts.Triggers
{
    public interface IAlertTrigger
    {
        bool ShouldTrigger(AlertTriggerDefinition triggerDefinition, Miner miner);
        Task<TriggerResult> ExecuteTriggerAsync(AlertTriggerDefinition triggerDefinition, AlertDefinition alertDefinition, Miner miner, Alert alert, CancellationToken token);
    }
}