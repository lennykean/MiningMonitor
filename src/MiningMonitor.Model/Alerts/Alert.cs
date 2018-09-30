using System;
using System.Collections.Generic;

using LiteDB;

using Newtonsoft.Json;

namespace MiningMonitor.Model.Alerts
{
    public class Alert
    {
        [BsonId(autoId: false)]
        public Guid Id { get; set; }
        public Guid MinerId { get; set; }
        public Guid AlertDefinitionId { get; set; }
        public AlertSeverity Severity { get; set; }
        public string Message { get; set; }
        [JsonIgnore]
        public Dictionary<string, string> Metadata { get; set; }
        public DateTime Start { get; set; }
        public DateTime LastActive { get; set; }
        public DateTime? End { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
        [BsonIgnore]
        public bool Acknowledged => AcknowledgedAt != null;
        [BsonIgnore]
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
