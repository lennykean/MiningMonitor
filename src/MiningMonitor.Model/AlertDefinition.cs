using System;
using System.Collections.Generic;

using LiteDB;

namespace MiningMonitor.Model
{
    public class AlertDefinition
    {
        private Dictionary<string, string> _parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        [BsonId(false)]
        public Guid Id { get; set; }
        public Guid MinerId { get; set; }
        public AlertType AlertType { get; set; }
        public bool Enabled { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? LastEnabled { get; set; }
        public DateTime? LastTest { get; set; }

        public Dictionary<string, string> Parameters
        {
            get => _parameters;
            set => _parameters = new Dictionary<string, string>(value, StringComparer.OrdinalIgnoreCase);
        }
    }
}