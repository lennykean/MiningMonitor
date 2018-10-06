using System.Collections.Generic;
using System.Linq;

using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Model
{
    public class ScanResult
    {

        public ScanResult(IEnumerable<Alert> alerts)
        {
            Alerts = alerts;
        }

        public ScanResult() : this(Enumerable.Empty<Alert>())
        {
        }

        public IEnumerable<Alert> Alerts { get; }
        public bool IsSuccess => !Alerts.Any();

        public static ScanResult Success => new ScanResult();
        public static ScanResult Fail(IEnumerable<Alert> alerts) => new ScanResult(alerts);

    }
}