using System;

using LiteDB;

namespace MiningMonitor.Model
{
    public class Alert
    {
        [BsonId(false)]
        public Guid Id { get; set; }
        public Guid MinerId { get; set; }
        public Guid AlertDefinitionId { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public bool Acknowledged { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
    }
}
