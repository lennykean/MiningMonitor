using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;
using MiningMonitor.Model.Alerts.Actions;

namespace MiningMonitor.Alerts.Action
{
    public class WebHookAlertActionExecutor : IAlertActionExecutor
    {
        private readonly Func<HttpClient> _clientFactory;

        public WebHookAlertActionExecutor(Func<HttpClient> clientFactory)
        {
            _clientFactory = clientFactory;
        }
        
        public bool ShouldExecute(AlertActionDefinition actionDefinition, Miner miner)
        {
            return actionDefinition.Type == AlertActionType.WebHook;
        }

        public async Task<AlertActionResult> ExecuteActionAsync(AlertActionDefinition actionDefinition, AlertDefinition alertDefinition, Miner miner, AlertMetadata metadata, CancellationToken token)
        {
            var action = (WebHookAlertActionDefinition)actionDefinition;
            var client = _clientFactory();

            if (action.Body == null)
                await client.PostAsync(action.Url, null, token);
            else
                await client.PostAsync(action.Url, new StringContent(action.Body, Encoding.UTF8, "application/json"), token);

            return AlertActionResult.Complete(actionDefinition.Name, "Sent Web Hook");
        }
    }
}