using Newtonsoft.Json;

using Mongo = MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model.Alerts
{
    [Mongo.BsonKnownTypes(
        typeof(DisableGpuAlertTriggerDefinition),
        typeof(RestartMinerAlertTriggerDefinition),
        typeof(WebHookAlertTriggerDefinition))]
    public abstract class AlertTriggerDefinition
    {
        public string DisplayName { get; set; }
        public abstract TriggerType Type { get; }
        public abstract bool AllowRemote { get; }
        [JsonIgnore]
        [LiteDB.BsonIgnore, Mongo.BsonIgnore]
        public abstract string FriendlyName { get; }
        [LiteDB.BsonIgnore, Mongo.BsonIgnore]
        public string Name => DisplayName ?? FriendlyName;
    }
}
