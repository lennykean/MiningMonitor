namespace MiningMonitor.Model.Alerts
{
    public class AlertThresholdMetadata
    {
        public Metric? GpuMetric { get; set; }
        public decimal? Min { get; set; }
        public decimal? Max { get; set; }
    }
}