using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;
using MiningMonitor.Model.Alerts.Actions;

namespace MiningMonitor.Alerts.Action
{
    public interface IAlertActionExecutor
    {
        bool ShouldExecute(AlertActionDefinition actionDefinition, Miner miner);
        Task<AlertActionResult> ExecuteActionAsync(AlertActionDefinition actionDefinition, AlertDefinition alertDefinition, Miner miner, AlertMetadata metadata, CancellationToken token);
    }
}