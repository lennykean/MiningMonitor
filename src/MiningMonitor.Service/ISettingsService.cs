using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiningMonitor.Service
{
    public interface ISettingsService
    {
        IDictionary<string, string> GetAll();
        (bool success, string setting) GetSetting(string key);
        (bool success, IDictionary<string, string> settings) UpdateSettings(IDictionary<string, string> settings);
        bool UpdateSetting(string setting, string value);
    }
}