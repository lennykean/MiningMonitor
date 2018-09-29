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
        public string DisplayName { get; set; }
        public bool Enabled { get; set; }
        [JsonConverter(typeof(AlertParametersConverter))]
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
                
                switch (Parameters.AlertType)
                {
                    case AlertType.Hashrate:
                        return "Hashrate alert";
                    case AlertType.GpuThreshold:
                        return "GPU threshold alert";
                    case AlertType.Connectivity:
                        return "Connectivity alert";
                    default:
                        return $"{Parameters.AlertType} alert";
                }
            }
        }

        [JsonIgnore, BsonIgnore]
        public DateTime NextScanStartTime
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