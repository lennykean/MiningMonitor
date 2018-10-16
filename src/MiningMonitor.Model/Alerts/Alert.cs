using System;
using System.Collections.Generic;

using MiningMonitor.Model.Alerts.Actions;

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
        public IEnumerable<AlertActionResult> ActionResults { get; set; }
        public DateTime Start { get; set; }
        public DateTime LastActive { get; set; }
        public DateTime? End { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
        [LiteDB.BsonIgnore, Mongo.BsonIgnore]
        public bool Acknowledged => AcknowledgedAt != null;
        [LiteDB.BsonIgnore, Mongo.BsonIgnore]
        public bool Active => End == null;
    }
}
