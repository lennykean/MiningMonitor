using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

using LiteDB;

using MiningMonitor.Common;
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
        public DateTime NoScanBefore
        {
            get
            {
                if (Updated > Created)
                    return (DateTime)Updated;

                return Created;
            }
        }

        [JsonIgnore, BsonIgnore]
        public DateTime NeedsScanAfter
        {
            get
            {
                if (LastScan > NoScanBefore)
                    return (DateTime)LastScan;

                return NoScanBefore;
            }
        }

        public ConcretePeriod NextScanPeriod(DateTime scanTime, TimeSpan? requestedDuration = null)
        {
            if (scanTime - requestedDuration >= NoScanBefore)
            {
                if (scanTime - requestedDuration >= NeedsScanAfter)
                {
                    return new ConcretePeriod(NeedsScanAfter, scanTime);
                }
                return new ConcretePeriod(scanTime - (TimeSpan)requestedDuration, scanTime);
            }
            return new ConcretePeriod(NeedsScanAfter, scanTime);
        }
    }
}