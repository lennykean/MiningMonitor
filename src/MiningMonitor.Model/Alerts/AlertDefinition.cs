using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

using LiteDB;

using MiningMonitor.Model.Serialization;
using MiningMonitor.Model.Validation;

using Newtonsoft.Json;

namespace MiningMonitor.Model.Alerts
{
    public class AlertDefinition
    {
        [BsonId(autoId: false)]
        public Guid Id { get; set; }
        [RequiredGuid(ErrorMessage = "Miner is required")]
        public Guid MinerId { get; set; }
        [Required(ErrorMessage = "Severity is required")]
        public AlertSeverity? Severity { get; set; }
        public string DisplayName { get; set; }
        public bool Enabled { get; set; }
        [JsonConverter(typeof(AlertParametersConverter))]
        [Required(ErrorMessage = "Alert parameters are required")]
        public AlertParameters Parameters { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? LastEnabled { get; set; }
        public DateTime? LastScan { get; set; }

        [BsonIgnore]        
        public string Name
        {
            get 
            {
                if (DisplayName != null)
                    return DisplayName; 
                if (Parameters == null)
                    return null;

                return $"{Regex.Replace(Parameters.AlertType.ToString(), "([A-Z][a-z])", " $1")} alert";
            }
        }
        

        [JsonIgnore, BsonIgnore]
        public DateTime LastScanEnd
        {
            get
            {
                if (LastEnabled > LastScan)
                    return (DateTime)LastEnabled;

                return LastScan ?? Created;
            }
        }
    }
}