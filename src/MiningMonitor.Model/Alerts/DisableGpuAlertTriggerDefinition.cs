using MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model.Alerts
{
    [BsonDiscriminator("DisableGpu")]
    public class DisableGpuAlertTriggerDefinition : AlertTriggerDefinition
    {
        public override TriggerType Type => TriggerType.DisableGpu;
        public override bool AllowRemote => false;
        public bool DisableAll { get; set; }
        public bool DisableAffected { get; set; }
        public override string FriendlyName => "Disable GPU";
    }
}