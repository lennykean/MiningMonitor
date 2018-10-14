using MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model.Alerts
{
    [BsonDiscriminator("WebHook")]
    public class WebHookAlertTriggerDefinition : AlertTriggerDefinition
    {
        public override TriggerType Type => TriggerType.WebHook;
        public override bool AllowRemote => true;
        public override string FriendlyName => "Web Hook";
        public string Url { get; set; }
        public string Body { get; set; }
    }
}