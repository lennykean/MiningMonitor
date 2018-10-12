using MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model.Alerts
{
    [BsonKnownTypes(typeof(ConnectivityAlertParameters), typeof(HashrateAlertParameters), typeof(GpuThresholdAlertParameters))]
    public abstract class AlertParameters
    {
        public abstract AlertType AlertType { get; }
        public string AlertMessage { get; set; }
        public int? DurationMinutes { get; set; }
    }
}
