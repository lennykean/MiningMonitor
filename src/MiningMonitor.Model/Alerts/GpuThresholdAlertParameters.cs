using MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model.Alerts
{
    [BsonDiscriminator("GpuThreshold")]
    public class GpuThresholdAlertParameters : AlertParameters
    {
        public override AlertType AlertType => AlertType.GpuThreshold;
        public Metric? Metric { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
    }
}
