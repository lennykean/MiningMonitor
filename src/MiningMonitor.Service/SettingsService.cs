using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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

        public IDictionary<string, string> GetAll()
        {
            var settings = _collection.FindAll();

            var mergedSettings = DefaultSettings.ToDictionary(
                defaultSetting => defaultSetting.Key,
                defaultSetting => (
                    from setting in settings
                    where setting.Key.Equals(defaultSetting.Key, StringComparison.OrdinalIgnoreCase)
                    select setting.Value).FirstOrDefault() ?? defaultSetting.Value);

            return mergedSettings;
        }

        public (bool success, string setting) GetSetting(string key)
        {
            if (!DefaultSettings.ContainsKey(key))
                return (false, null);

            DefaultSettings.TryGetKey(key, out var normalizedKey);

            var setting = _collection.FindOne(s => s.Key.ToLower() == normalizedKey.ToLower());
            var value = setting?.Value ?? DefaultSettings[normalizedKey];

            return (true, value);
        }

        public (bool success, IDictionary<string, string> settings) UpdateSettings(IDictionary<string, string> settings)
        {
            if (settings.Any(s => !DefaultSettings.ContainsKey(s.Key)))
                return (false, null);

            foreach (var setting in settings)
            {
                DefaultSettings.TryGetKey(setting.Key, out var normalizedKey);
                var originalSetting = _collection.FindOne(s => s.Key.ToLower() == normalizedKey.ToLower());
                if (originalSetting == null)
                {
                    _collection.Insert(new Setting { Key = setting.Key, Value = setting.Value });
                }
                else
                {
                    originalSetting.Value = setting.Value;
                    _collection.Update(originalSetting);
                }
            }
            return (true, GetAll());
        }

        public bool UpdateSetting(string setting, string value)
        {
            if (!DefaultSettings.TryGetKey(setting, out var normalizedKey))
                return false;

            var originalSetting = _collection.FindOne(s => s.Key.ToLower() == normalizedKey.ToLower());
            if (originalSetting == null)
            {
                _collection.Insert(new Setting { Key = setting, Value = value });
            }
            else
            {
                originalSetting.Value = value;
                _collection.Update(originalSetting);
            }
            return true;
        }
    }
}