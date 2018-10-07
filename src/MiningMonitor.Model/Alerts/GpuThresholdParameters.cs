namespace MiningMonitor.Model.Alerts
{
    public class GpuThresholdParameters : AlertParameters
    {
        public override AlertType AlertType => AlertType.GpuThreshold;
        public Metric? Metric { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
    }
}
