using System;

using ClaymoreMiner.RemoteManagement.Models;

using Newtonsoft.Json;

using Mongo = MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model
{
    public class Snapshot
    {
        [LiteDB.BsonId(autoId: false), Mongo.BsonId]
        public Guid Id { get; set; }
        public Guid MinerId { get; set; }
        public DateTime SnapshotTime { get; set; }
        public TimeSpan RetrievalElapsedTime { get; set; }
        [JsonIgnore]
        public bool? IsSynced { get; set; }
        public MinerStatistics MinerStatistics { get; set; }
    }
}