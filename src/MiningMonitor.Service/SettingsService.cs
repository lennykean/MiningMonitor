using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Data;
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
            ["CollectorId"] = null,
            ["EnablePurge"] = "true",
            ["PurgeAgeMinutes"] = "1440",
        }.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);

        private readonly IRepository<Setting> _collection;

        public SettingsService(IRepository<Setting> collection)
        {
            _collection = collection;
        }

        public async Task<IDictionary<string, string>> GetAllAsync(CancellationToken token = default)
        {
            var settings = await _collection.FindAllAsync(token);

            var mergedSettings = DefaultSettings.ToDictionary(
                defaultSetting => defaultSetting.Key,
                defaultSetting => (
                    from setting in settings
                    where setting.Key.Equals(defaultSetting.Key, StringComparison.OrdinalIgnoreCase)
                    select setting.Value).FirstOrDefault() ?? defaultSetting.Value);

            return mergedSettings;
        }

        public async Task<(bool success, string setting)> GetSettingAsync(string key, CancellationToken token = default)
        {
            if (!DefaultSettings.ContainsKey(key))
                return (false, null);

            DefaultSettings.TryGetKey(key, out var normalizedKey);

            var setting = await _collection.FindOneAsync(s => s.Key.ToLower() == normalizedKey.ToLower(), token);
            var value = setting?.Value ?? DefaultSettings[normalizedKey];

            return (true, value);
        }

        public async Task<(bool success, IDictionary<string, string> settings)> UpdateSettingsAsync(IDictionary<string, string> settings, CancellationToken token = default)
        {
            if (settings.Any(s => !DefaultSettings.ContainsKey(s.Key)))
                return (false, null);

            foreach (var setting in settings)
            {
                DefaultSettings.TryGetKey(setting.Key, out var normalizedKey);
                var originalSetting = await _collection.FindOneAsync(s => s.Key.ToLower() == normalizedKey.ToLower(), token);
                if (originalSetting == null)
                {
                    await _collection.InsertAsync(new Setting { Key = setting.Key, Value = setting.Value }, token);
                }
                else
                {
                    originalSetting.Value = setting.Value;
                    await _collection.UpdateAsync(originalSetting, token);
                }
            }
            return (true, await GetAllAsync(token));
        }

        public async Task<bool> UpdateSettingAsync(string setting, string value, CancellationToken token = default)
        {
            if (!DefaultSettings.TryGetKey(setting, out var normalizedKey))
                return false;

            var originalSetting = await _collection.FindOneAsync(s => s.Key.ToLower() == normalizedKey.ToLower(), token);
            if (originalSetting == null)
            {
                await _collection.InsertAsync(new Setting { Key = setting, Value = value }, token);
            }
            else
            {
                originalSetting.Value = value;
                await _collection.UpdateAsync(originalSetting, token);
            }
            return true;
        }
    }
}