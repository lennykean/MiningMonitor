using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using MiningMonitor.Data;
using MiningMonitor.Model;
using MiningMonitor.Model.Identity;

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

        private readonly IRepository<Setting, string> _settingsRepo;
        private readonly IRepository<MiningMonitorUser, Guid> _userRepo;

        public SettingsService(IRepository<Setting, string> settingsRepo, IRepository<MiningMonitorUser, Guid> userRepo)
        {
            _settingsRepo = settingsRepo;
            _userRepo = userRepo;
        }

        public async Task<IDictionary<string, string>> GetAllAsync(CancellationToken token = default)
        {
            var settings = (await _settingsRepo.FindAllAsync(token)).ToList();

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

            var setting = await _settingsRepo.FindOneAsync(s => s.Key.ToLower() == normalizedKey.ToLower(), token);
            var value = setting?.Value ?? DefaultSettings[normalizedKey];

            return (true, value);
        }

        public async Task<(ModelStateDictionary modelState, IDictionary<string, string> settings)> UpdateSettingsAsync(IDictionary<string, string> settings, CancellationToken token = default)
        {
            var modelState = new ModelStateDictionary();
            
            foreach (var setting in settings)
            {
                if (!DefaultSettings.TryGetKey(setting.Key, out var normalizedKey))
                {
                    modelState.AddModelError(setting.Key, "Setting does not exist");
                    continue;
                }
                if (normalizedKey == "ServerToken" || normalizedKey == "CollectorId")
                {
                    continue;
                }
                modelState.Merge(await UpdateSettingAsync(setting.Key, setting.Value, token));
            }
            return (modelState, await GetAllAsync(token));
        }

        public async Task<ModelStateDictionary> UpdateSettingAsync(string setting, string value, CancellationToken token = default)
        {
            var modelState = new ModelStateDictionary();

            if (!DefaultSettings.TryGetKey(setting, out var normalizedKey))
            {
                modelState.AddModelError(setting, "Setting does not exist");
                return modelState;
            }
            if (normalizedKey == "EnableSecurity" && value == "true" && !(await _userRepo.FindAllAsync(token)).Any())
            {
                modelState.AddModelError(setting, "Cannot enable security, no users exist");
                return modelState;
            }
            if (normalizedKey == "IsDataCollector" && value == "false")
            {
                modelState.Merge(await UpdateSettingAsync("ServerToken", null, token));
                modelState.Merge(await UpdateSettingAsync("CollectorId", null, token));
                if (!modelState.IsValid)
                {
                    return modelState;
                }
            }
            await _settingsRepo.UpsertAsync(new Setting { Key = normalizedKey, Value = value }, token);
            
            return modelState;
        }
    }
}