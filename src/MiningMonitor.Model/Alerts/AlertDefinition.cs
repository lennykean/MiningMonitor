using System;

using LiteDB;

using MiningMonitor.Model.Serialization;

using Newtonsoft.Json;

namespace MiningMonitor.Model.Alerts
{
    public class AlertDefinition
    {
        [BsonId(autoId: false)]
        public Guid Id { get; set; }
        public Guid MinerId { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        [JsonConverter(typeof(AlertParametersConverter))]
        public AlertParameters Parameters { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? LastEnabled { get; set; }
        public DateTime? LastScan { get; set; }
    }
}