using MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model.Alerts
{
    [BsonDiscriminator("Connectivity")]
    public class ConnectivityAlertParameters : AlertParameters
    {
        public override AlertType AlertType => AlertType.Connectivity;
    }
}
