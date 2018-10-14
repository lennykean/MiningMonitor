using MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model.Alerts
{
    [BsonDiscriminator("RestartMiner")]
    public class RestartMinerAlertTriggerDefinition : AlertTriggerDefinition
    {
        public override TriggerType Type => TriggerType.RestartMiner;
        public override bool AllowRemote => false;
        public override string FriendlyName => "Restart Miner";
    }
}