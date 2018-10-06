using System;
using System.Collections.Generic;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service.Alerts.Scanners
{
    public interface IAlertScanner
    {
        bool ShouldScan(AlertDefinition definition);
        Period CalculateScanPeriod(AlertDefinition definition, DateTime scanTime);
        bool EndAlert(AlertDefinition definition, Miner miner, Alert alert, IEnumerable<Snapshot> snapshots, DateTime scanTime);
        ScanResult PerformScan(AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime);
    }
}
