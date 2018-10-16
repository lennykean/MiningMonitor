using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts
{
    public interface IAlertFactory
    {
        Task<Alert> CreateAlertAsync(AlertDefinition definition, Miner miner, AlertMetadata metadata, string defaultMessage, IEnumerable<string> detailMessages, CancellationToken token);
    }
}