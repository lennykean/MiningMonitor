using System.Threading.Tasks;

using LiteDB;

using MiningMonitor.Model;

namespace MiningMonitor.Data.Repository
{
    public class SettingRepository : LiteDbRepository<Setting, string>, ISettingRepository
    {
        public SettingRepository(LiteCollection<Setting> dbCollection) : base(dbCollection)
        {
        }

        public Task<Setting> GetSettingAsync(string key)
        {
            return Task.Run(() => DbCollection.FindOne(s => s.Key.ToLower() == key.ToLower()));
        }
    }
}