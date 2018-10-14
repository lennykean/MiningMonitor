using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

using MiningMonitor.Common;
using MiningMonitor.Model.Serialization;
using MiningMonitor.Model.Validation;

using Newtonsoft.Json;

using Mongo = MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model.Alerts
{
    public class AlertDefinition
    {
        [LiteDB.BsonId(autoId: false), Mongo.BsonId]
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
        [JsonProperty(ItemConverterType = typeof(AlertTriggerDefinitionConverter))]
        public List<AlertTriggerDefinition> Triggers { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? LastEnabled { get; set; }
        public DateTime? LastScan { get; set; }

        [LiteDB.BsonIgnore, Mongo.BsonIgnore]
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

        public ConcretePeriod NextScanPeriod(DateTime scanTime, TimeSpan? preScanOverFetch = null)
        {
            var scanTimeWithBuffer = scanTime - preScanOverFetch;

            if ((scanTimeWithBuffer < LastScan || LastScan == null) && scanTimeWithBuffer > (Updated ?? Created))
                return new ConcretePeriod((DateTime)scanTimeWithBuffer, scanTime);
            if (LastScan > (Updated ?? Created))
                return new ConcretePeriod((DateTime)LastScan, scanTime);

            return new ConcretePeriod(Updated ?? Created, scanTime);
        }
    }
}