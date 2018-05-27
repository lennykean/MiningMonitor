using System.Threading.Tasks;

using MiningMonitor.Model;

namespace MiningMonitor.Data.Repository
{
    public interface ISettingRepository : IRepository<Setting, string>
    {
        Task<Setting> GetSettingAsync(string key);
    }
}