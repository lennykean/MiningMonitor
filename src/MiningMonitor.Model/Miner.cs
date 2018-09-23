using System;
using System.ComponentModel.DataAnnotations;

using LiteDB;

using Newtonsoft.Json;

namespace MiningMonitor.Model
{
    public class Miner
    {
        [BsonId(autoId: false)]
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        [Required]
        public string Address { get; set; }
        [Range(1, ushort.MaxValue)]
        public int? Port { get; set; }
        public string Password { get; set; }
        public bool CollectData { get; set; }
        public string CollectorId { get; set; }
        [JsonIgnore]
        public bool? IsSynced { get; set; }

        [BsonIgnore]
        public string Name
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(DisplayName))
                    return DisplayName;

                return $"{Address}{(Port == null ? null : ":")}{Port}";
            }
        }
    }
}