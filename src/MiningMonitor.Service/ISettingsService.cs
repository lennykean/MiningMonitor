using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MiningMonitor.Service
{
    public interface ISettingsService
    {
        Task<IDictionary<string, string>> GetAllAsync(CancellationToken token = default);
        Task<(bool success, string setting)> GetSettingAsync(string key, CancellationToken token = default);
        Task<(ModelStateDictionary modelState, IDictionary<string, string> settings)> UpdateSettingsAsync(IDictionary<string, string> settings, CancellationToken token = default);
        Task<ModelStateDictionary> UpdateSettingAsync(string setting, string value, CancellationToken token = default);
    }
}