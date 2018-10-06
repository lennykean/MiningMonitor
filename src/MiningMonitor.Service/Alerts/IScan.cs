using System;
using System.Collections.Generic;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service.Alerts
{
    public interface IScan
    {
        AlertDefinition Definition { get; }
        Period ScanPeriod { get; }

        bool EndAlert(Alert alert, IEnumerable<Snapshot> snapshots);
        ScanResult PerformScan(IEnumerable<Snapshot> snapshots);
    }
}