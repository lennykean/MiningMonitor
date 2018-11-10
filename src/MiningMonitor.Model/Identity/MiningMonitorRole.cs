using System;

using Mongo = MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model.Identity
{
    public class MiningMonitorRole
    {
        [LiteDB.BsonId(autoId: false), Mongo.BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}
