using System;
using System.Collections.Generic;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;
using MiningMonitor.Service.Alerts.Scanners;

namespace MiningMonitor.Service.Alerts
{
    public class Scan : IScan
    {
        private readonly IAlertScanner _scanner;

        public Scan(AlertDefinition alertDefinition, IAlertScanner scanner)
        {
            Definition = alertDefinition;
            _scanner = scanner;
        }

        public AlertDefinition Definition { get; }
        public DateTime ScanStart => _scanner.CalculateScanStart(Definition);

        public bool EndAlert(Alert alert, IEnumerable<Snapshot> snapshots)
        {
            return _scanner.EndAlert(Definition, alert, snapshots);
        }

        public Alert PerformScan(IEnumerable<Snapshot> snapshots)
        {
            return _scanner.PerformScan(Definition, snapshots);
        }
    }
}
