using MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model.Alerts.Actions
{
    [BsonDiscriminator("RestartMiner")]
    public class RestartMinerAlertActionDefinition : AlertActionDefinition
    {
        public override AlertActionType Type => AlertActionType.RestartMiner;
        public override bool AllowRemote => false;
        public override string FriendlyName => "Restart Miner";
    }
}