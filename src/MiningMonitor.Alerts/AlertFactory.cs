using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MiningMonitor.Alerts.Action;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;
using MiningMonitor.Model.Alerts.Actions;

namespace MiningMonitor.Alerts
{
    public class AlertFactory : IAlertFactory
    {
        private readonly IEnumerable<IAlertActionExecutor> _alertActionExecutors;
        private readonly ILogger<AlertFactory> _logger;

        public AlertFactory(IEnumerable<IAlertActionExecutor> alertActionExecutors, ILogger<AlertFactory> logger)
        {
            _alertActionExecutors = alertActionExecutors;
            _logger = logger;
        }

        public async Task<Alert> CreateAlertAsync(AlertDefinition definition, Miner miner, AlertMetadata metadata, string defaultMessage, IEnumerable<string> detailMessages, CancellationToken token)
        {
            var now = DateTime.UtcNow;

            return new Alert
            {
                MinerId = definition.MinerId,
                AlertDefinitionId = definition.Id,
                Severity = definition.Severity ?? AlertSeverity.None,
                Message = definition.Parameters?.AlertMessage ?? defaultMessage,
                DetailMessages = detailMessages,
                Metadata = metadata,
                Start = now,
                LastActive = now,
                ActionResults = await ProcessActionsAsync(definition, miner, metadata, token)
            };
        }

        private async Task<IEnumerable<AlertActionResult>> ProcessActionsAsync(AlertDefinition definition, Miner miner, AlertMetadata metadata, CancellationToken token)
        {
            var results = new List<AlertActionResult>();
            if (!definition.Actions.Any())
                return results;

            _logger.LogInformation($"Processing actions for alert definition {definition.Id}");
            foreach (var action in definition.Actions)
            {
                foreach (var actionExecutor in _alertActionExecutors.Where(t => t.ShouldExecute(action, miner)))
                {
                    _logger.LogInformation($"Processing action {action.Name} for alert definition {definition.Id}");
                    try
                    {
                        results.Add(await actionExecutor.ExecuteActionAsync(action, definition, miner, metadata, token));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error executing action ({action.Name}) for alert definition {definition.Id}");
                        results.Add(new AlertActionResult
                        {
                            State = AlertActionState.Error,
                            ActionName = action.Name,
                            Message = "Error executing action"
                        });
                    }
                }
            }
            _logger.LogInformation($"Finished processing actions for alert definition {definition.Id}");

            return results;
        }
    }
}
