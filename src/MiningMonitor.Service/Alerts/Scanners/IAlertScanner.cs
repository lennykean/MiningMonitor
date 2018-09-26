using System;
using System.Collections.Generic;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service.Alerts.Scanners
{
    public interface IAlertScanner
    {
        bool ShouldScan(AlertDefinition definition);
        DateTime CalculateScanStart(AlertDefinition definition);
        bool EndAlert(AlertDefinition definition, Alert alert, IEnumerable<Snapshot> snapshots);
        Alert PerformScan(AlertDefinition definition, IEnumerable<Snapshot> snapshots);
    }
}
