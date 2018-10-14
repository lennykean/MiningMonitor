using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts.Triggers
{
    public interface ITriggerProcessor
    {
        Task<IEnumerable<TriggerResult>> ProcessTriggersAsync(AlertDefinition alertDefinition, Alert alert, Miner miner, CancellationToken token);
    }
}