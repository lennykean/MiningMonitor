using MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model.Alerts
{
    [BsonDiscriminator("Hashrate")]
    public class HashrateAlertParameters : AlertParameters
    {
        public override AlertType AlertType => AlertType.Hashrate;
        public decimal? MinValue { get; set; }
    }
}
