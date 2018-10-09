using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Mongo = MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model.Alerts
{
    public class Alert
    {
        [LiteDB.BsonId(autoId: false), Mongo.BsonId]
        public Guid Id { get; set; }
        public Guid MinerId { get; set; }
        public Guid AlertDefinitionId { get; set; }
        public AlertSeverity Severity { get; set; }
        public string Message { get; set; }
        [JsonIgnore]
        public AlertMetadata Metadata { get; set; }
        public IEnumerable<string> DetailMessages { get; set; }
        public DateTime Start { get; set; }
        public DateTime LastActive { get; set; }
        public DateTime? End { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
        [LiteDB.BsonIgnore, Mongo.BsonIgnore]
        public bool Acknowledged => AcknowledgedAt != null;
        [LiteDB.BsonIgnore, Mongo.BsonIgnore]
        public bool Active => End == null;

        public static Alert CreateFromDefinition(AlertDefinition definition, string message)
        {
            return new Alert
            {
                MinerId = definition.MinerId,
                AlertDefinitionId = definition.Id,
                Severity = definition.Severity ?? AlertSeverity.None,
                Message = message
            };
        }
    }
}
