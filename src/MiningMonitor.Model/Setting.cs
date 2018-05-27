using LiteDB;

namespace MiningMonitor.Model
{
    public class Setting
    {
        [BsonId]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}