using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiningMonitor.Service
{
    public interface ISettingsService
    {
        Task<IDictionary<string, string>> GetAllAsync();

        Task<(bool success, string setting)> GetSettingAsync(string key);

        Task<(bool success, IDictionary<string, string> settings)> UpdateSettingsAsync(IDictionary<string, string> settings);

        Task<bool> UpdateSettingAsync(string setting, string value);
    }
}