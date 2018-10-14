using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Alerts.Scanners;
using MiningMonitor.Alerts.Triggers;
using MiningMonitor.Common;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts
{
    public class Scan : IScan
    {
        private readonly Miner _miner;
        private readonly IAlertScanner _scanner;
        private readonly ITriggerProcessor _triggerProcessor;
        private readonly DateTime _scanTime;

        public Scan(AlertDefinition alertDefinition, Miner miner, IAlertScanner scanner, ITriggerProcessor triggerProcessor, DateTime scanTime)
        {
            Definition = alertDefinition;
            _miner = miner;
            _scanner = scanner;
            _triggerProcessor = triggerProcessor;
            _scanTime = scanTime;
        }

        public AlertDefinition Definition { get; }
        public Period ScanPeriod => _scanner.CalculateScanPeriod(Definition, _scanTime);

        public bool EndAlert(Alert alert, IEnumerable<Snapshot> snapshots)
        {
            return _scanner.EndAlert(Definition, _miner, alert, snapshots, _scanTime);
        }

        public async Task<ScanResult> PerformScanAsync(IEnumerable<Alert> activeAlerts, IEnumerable<Snapshot> snapshots, CancellationToken token)
        {
            var scanResult = _scanner.PerformScan(activeAlerts, Definition, _miner, snapshots, _scanTime);

            foreach (var alert in scanResult.Alerts)
                alert.TriggerResults = await _triggerProcessor.ProcessTriggersAsync(Definition, alert, _miner, token);

            return scanResult;
        }
    }
}
