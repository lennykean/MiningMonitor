using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Common;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts.Scanners
{
    public interface IAlertScanner
    {
        bool ShouldScan(AlertDefinition definition);
        Period CalculateScanPeriod(AlertDefinition definition, DateTime scanTime);
        bool EndAlert(AlertDefinition definition, Miner miner, Alert alert, IEnumerable<Snapshot> snapshots, DateTime scanTime);
        Task<ScanResult> PerformScanAsync(IEnumerable<Alert> activeAlerts, AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime, CancellationToken token);
    }
}
