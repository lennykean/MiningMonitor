using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts.Triggers
{
    public class WebHookAlertTrigger : IAlertTrigger
    {
        private readonly Func<HttpClient> _clientFactory;

        public WebHookAlertTrigger(Func<HttpClient> clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public bool ShouldTrigger(AlertTriggerDefinition triggerDefinition, Miner miner)
        {
            return triggerDefinition.Type == TriggerType.WebHook;
        }

        public async Task<TriggerResult> ExecuteTriggerAsync(AlertTriggerDefinition triggerDefinition, AlertDefinition alertDefinition, Miner miner, Alert alert, CancellationToken token)
        {
            var trigger = (WebHookAlertTriggerDefinition)triggerDefinition;
            var client = _clientFactory();

            if (trigger.Body == null)
                await client.PostAsJsonAsync(trigger.Url, alert, token);
            else
                await client.PostAsync(trigger.Url, new StringContent(trigger.Body, Encoding.UTF8, "application/json"), token);

            return TriggerResult.Complete($"Sent Web Hook", triggerDefinition.Name);
        }
    }
}