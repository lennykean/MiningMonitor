using System;
using System.Collections.Generic;
using System.Linq;

using MiningMonitor.Alerts.Scanners;
using MiningMonitor.Alerts.Triggers;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts
{
    public class ScanFactory : IScanFactory
    {
        private readonly IEnumerable<IAlertScanner> _scanners;
        private readonly ITriggerProcessor _triggerProcessor;

        public ScanFactory(IEnumerable<IAlertScanner> scanners, ITriggerProcessor triggerProcessor)
        {
            _scanners = scanners;
            _triggerProcessor = triggerProcessor;
        }

        public IScan CreateScan(AlertDefinition definition, Miner miner, DateTime scanTime)
        {
            return new Scan(definition, miner, _scanners.Single(s => s.ShouldScan(definition)), _triggerProcessor, scanTime);
        }
    }
}