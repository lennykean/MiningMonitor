using System.Collections.Generic;

using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts
{
    public class ScanResult
    {
        private ScanResult(bool skip)
        {
            Skipped = skip;
        }

        private ScanResult(bool skip, bool success) : this(skip)
        {
            Succeeded = !skip && success;
            Failed = !skip && !success;
        }

        private ScanResult(IEnumerable<Alert> alerts, bool success) : this(false, success)
        {
            Alerts = alerts;
        }
        
        public IEnumerable<Alert> Alerts { get; }
        public bool Succeeded { get; }
        public bool Failed { get; }
        public bool Skipped { get; }

        public static ScanResult Skip => new ScanResult(skip: true);
        public static ScanResult Success => new ScanResult(skip: false, success: true);
        public static ScanResult Fail(params Alert[] alerts) => new ScanResult(alerts, success: false);

    }
}