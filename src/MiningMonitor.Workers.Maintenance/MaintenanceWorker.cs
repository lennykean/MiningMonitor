using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MiningMonitor.Service;

namespace MiningMonitor.Workers.Maintenance
{
    public class MaintenanceWorker : IWorker
    {
        private readonly ISettingsService _settingsService;
        private readonly ISnapshotService _snapshotService;
        private readonly IAlertService _alertService;
        private readonly ILogger<MaintenanceWorker> _logger;

        public MaintenanceWorker(
            ISettingsService settingsService, 
            ISnapshotService snapshotService, 
            IAlertService alertService,
            ILogger<MaintenanceWorker> logger)
        {
            _settingsService = settingsService;
            _snapshotService = snapshotService;
            _alertService = alertService;
            _logger = logger;
        }

        public async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            var (_, enablePurgeSetting) = await _settingsService.GetSettingAsync("EnablePurge", cancellationToken);
            var (_, purgeAgeMinutesSetting) = await _settingsService.GetSettingAsync("PurgeAgeMinutes", cancellationToken);

            if (!bool.TryParse(enablePurgeSetting, out var enablePurge) || !int.TryParse(purgeAgeMinutesSetting, out var purgeAgeMinutes) || !enablePurge)
            {
                _logger.LogInformation("Data purge is disabled, skipping.");
                return;
            }

            var purgeCutoff = DateTime.UtcNow - TimeSpan.FromMinutes(purgeAgeMinutes);
            _logger.LogInformation($"Purging snapshot and alert data before {purgeCutoff:MM/dd/yy H:mm}");

            var snapshotPurgedCount = await _snapshotService.DeleteOldAsync(purgeCutoff, cancellationToken);
            _logger.LogInformation($"Purged {snapshotPurgedCount} snapshot(s).");

            var alertPurgedCount = await _alertService.DeleteOldAsync(purgeCutoff, cancellationToken);
            _logger.LogInformation($"Purged {alertPurgedCount} alert(s).");
        }
    }
}