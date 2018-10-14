using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts.Triggers
{
    public class TriggerProcessor : ITriggerProcessor
    {
        private readonly IEnumerable<IAlertTrigger> _triggers;
        private readonly ILogger<TriggerProcessor> _logger;

        public TriggerProcessor(IEnumerable<IAlertTrigger> triggers, ILogger<TriggerProcessor> logger)
        {
            _triggers = triggers;
            _logger = logger;
        }

        public async Task<IEnumerable<TriggerResult>> ProcessTriggersAsync(AlertDefinition alertDefinition, Alert alert, Miner miner, CancellationToken token)
        {
            var results = new List<TriggerResult>();
            if (!alertDefinition.Triggers.Any())
                return results;
            
            _logger.LogInformation($"Processing triggers for alert definition {alertDefinition.Id}");
            foreach (var triggerDefinition in alertDefinition.Triggers)
            {
                foreach (var trigger in _triggers.Where(t => t.ShouldTrigger(triggerDefinition, miner)))
                {
                    _logger.LogInformation($"Processing trigger {triggerDefinition.Name} for alert definition {alertDefinition.Id}");
                    try
                    {
                        results.Add(await trigger.ExecuteTriggerAsync(triggerDefinition, alertDefinition, miner, alert, token));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error executing trigger ({triggerDefinition.Name}) for alert definition {alertDefinition.Id}");
                        results.Add(new TriggerResult
                        {
                            State = TriggerState.Error,
                            TriggerName = triggerDefinition.Name,
                            Message = "Error executing trigger"
                        });
                    }
                }
            }
            _logger.LogInformation($"Finished processing triggers for alert definition {alertDefinition.Id}");

            return results;
        }
    }
}
