using MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model.Alerts.Actions
{
    [BsonDiscriminator("WebHook")]
    public class WebHookAlertActionDefinition : AlertActionDefinition
    {
        public override AlertActionType Type => AlertActionType.WebHook;
        public override bool AllowRemote => true;
        public override string FriendlyName => "Web Hook";
        public string Url { get; set; }
        public string Body { get; set; }
    }
}