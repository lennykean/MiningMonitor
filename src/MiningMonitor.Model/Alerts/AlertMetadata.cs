namespace MiningMonitor.Model.Alerts
{
    public class AlertMetadata
    {
        public int? GpuIndex { get; set; }
        public AlertThresholdMetadata Threshold { get; set; }
        public AlertValueMetadata Value { get; set; }
    }
}