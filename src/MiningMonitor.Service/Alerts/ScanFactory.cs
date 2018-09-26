using System.Collections.Generic;
using System.Linq;

using MiningMonitor.Model.Alerts;
using MiningMonitor.Service.Alerts.Scanners;

namespace MiningMonitor.Service.Alerts
{
    public class ScanFactory : IScanFactory
    {
        private readonly IEnumerable<IAlertScanner> _scanners;

        public ScanFactory(IEnumerable<IAlertScanner> scanners)
        {
            _scanners = scanners;
        }

        public IScan CreateScan(AlertDefinition definition)
        {
            return new Scan(definition, _scanners.Single(s => s.ShouldScan(definition)));
        }
    }
}