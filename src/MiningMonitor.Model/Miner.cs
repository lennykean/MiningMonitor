using System;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Mongo = MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model
{
    public class Miner
    {
        [LiteDB.BsonId(autoId: false), Mongo.BsonId]
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

        [LiteDB.BsonIgnore, Mongo.BsonIgnore]
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