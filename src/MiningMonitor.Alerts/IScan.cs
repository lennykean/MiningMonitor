using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        Task<ScanResult> PerformScanAsync(IEnumerable<Alert> activeAlerts, IEnumerable<Snapshot> snapshots, CancellationToken token);
    }
}