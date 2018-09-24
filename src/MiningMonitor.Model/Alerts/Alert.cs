using System;

using LiteDB;

namespace MiningMonitor.Model.Alerts
{
    public class Alert
    {
        [BsonId(autoId: false)]
        public Guid Id { get; set; }
        public Guid MinerId { get; set; }
        public Guid AlertDefinitionId { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
        [BsonIgnore]
        public bool Acknowledged => AcknowledgedAt != null;
        [BsonIgnore]
        public bool Active => End == null;
    }
}
