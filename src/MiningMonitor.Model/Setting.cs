using Mongo = MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model
{
    public class Setting
    {
        [LiteDB.BsonId, Mongo.BsonId]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}