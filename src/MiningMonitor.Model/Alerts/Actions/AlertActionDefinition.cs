using Newtonsoft.Json;

using Mongo = MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model.Alerts.Actions
{
    [Mongo.BsonKnownTypes(
        typeof(DisableGpuAlertActionDefinition),
        typeof(RestartMinerAlertActionDefinition),
        typeof(WebHookAlertActionDefinition))]
    public abstract class AlertActionDefinition
    {
        public string DisplayName { get; set; }
        public abstract AlertActionType Type { get; }
        public abstract bool AllowRemote { get; }
        [JsonIgnore]
        [LiteDB.BsonIgnore, Mongo.BsonIgnore]
        public abstract string FriendlyName { get; }
        [LiteDB.BsonIgnore, Mongo.BsonIgnore]
        public string Name => DisplayName ?? FriendlyName;
    }
}
