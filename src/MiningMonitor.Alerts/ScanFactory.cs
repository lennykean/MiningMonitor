using System;
using System.Collections.Generic;
using System.Linq;

using MiningMonitor.Alerts.Scanners;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts
{
    public class ScanFactory : IScanFactory
    {
        private readonly IEnumerable<IAlertScanner> _scanners;

        public ScanFactory(IEnumerable<IAlertScanner> scanners)
        {
            _scanners = scanners;
        }

        public IScan CreateScan(AlertDefinition definition, Miner miner, DateTime scanTime)
        {
            return new Scan(definition, miner, _scanners.Single(s => s.ShouldScan(definition)), scanTime);
        }
    }
}