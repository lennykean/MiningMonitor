using MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model.Alerts.Actions
{
    [BsonDiscriminator("DisableGpu")]
    public class DisableGpuAlertActionDefinition : AlertActionDefinition
    {
        public override AlertActionType Type => AlertActionType.DisableGpu;
        public override bool AllowRemote => false;
        public bool DisableAll { get; set; }
        public bool DisableAffected { get; set; }
        public override string FriendlyName => "Disable GPU";
    }
}