using System.Collections.Generic;

using MiningMonitor.Common;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts
{
    public interface IScan
    {
        AlertDefinition Definition { get; }
        Period ScanPeriod { get; }

        bool EndAlert(Alert alert, IEnumerable<Snapshot> snapshots);
        ScanResult PerformScan(IEnumerable<Alert> activeAlerts, IEnumerable<Snapshot> snapshots);
    }
}