using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

using MiningMonitor.Data.Repository;
using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public class SettingsService : ISettingsService
    {
        public static readonly ImmutableDictionary<string, string> DefaultSettings = new Dictionary<string, string>
        {
            ["EnableSecurity"] = "false",
            ["IsDataCollector"] = "false",
            ["ServerUrl"] = null,
            ["ServerToken"] = null,
            ["Name"] = null,
            ["CollectorId"] = null
        }.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);

        private readonly ISettingRepository _repository;

        public SettingsService(ISettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<IDictionary<string, string>> GetAllAsync()
        {
            var settings = await _repository.GetAllAsync();

            var mergedSettings = DefaultSettings.ToDictionary(
                defaultSetting => defaultSetting.Key,
                defaultSetting => (
                    from setting in settings
                    where setting.Key.Equals(defaultSetting.Key, StringComparison.OrdinalIgnoreCase)
                    select setting.Value).FirstOrDefault() ?? defaultSetting.Value);

            return mergedSettings;
        }

        public async Task<(bool success, string setting)> GetSettingAsync(string key)
        {
            if (!DefaultSettings.ContainsKey(key))
                return (false, null);

            DefaultSettings.TryGetKey(key, out var normalizedKey);

            var setting = await _repository.GetSettingAsync(normalizedKey);
            var value = setting?.Value ?? DefaultSettings[normalizedKey];

            return (true, value);
        }

        public async Task<(bool success, IDictionary<string, string> settings)> UpdateSettingsAsync(IDictionary<string, string> settings)
        {
            if (settings.Any(s => !DefaultSettings.ContainsKey(s.Key)))
                return (false, null);

            foreach (var setting in settings)
            {
                DefaultSettings.TryGetKey(setting.Key, out var normalizedKey);
                var originalSetting = await _repository.GetSettingAsync(normalizedKey);
                if (originalSetting == null)
                {
                    await _repository.AddAsync(new Setting {Key = setting.Key, Value = setting.Value});
                }
                else
                {
                    originalSetting.Value = setting.Value;
                    await _repository.UpdateAsync(originalSetting);
                }
            }

            return (true, await GetAllAsync());
        }
    }
}