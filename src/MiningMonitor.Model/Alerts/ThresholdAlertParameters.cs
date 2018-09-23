namespace MiningMonitor.Model.Alerts
{
    public class ThresholdAlertParameters : AlertParameters
    {
        public override AlertType AlertType => AlertType.Threshold;
        public Metric Metric { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
    }
}
