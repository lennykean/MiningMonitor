using System.Collections.Generic;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service.Alerts
{
    public interface IAlertScanner
    {
        bool ShouldScan(AlertDefinition definition);
        (bool alert, string description) Scan(AlertDefinition definition, IEnumerable<Snapshot> snapshots);
    }
}
