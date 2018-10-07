﻿using System;

using Microsoft.Extensions.Logging;

using MiningMonitor.Service;

namespace MiningMonitor.Workers.Maintenance
{
    public class MaintenanceWorker : SynchronousWorker
    {
        private readonly ISettingsService _settingsService;
        private readonly ISnapshotService _snapshotService;
        private readonly ILogger<MaintenanceWorker> _logger;

        public MaintenanceWorker(ISettingsService settingsService, ISnapshotService snapshotService, ILogger<MaintenanceWorker> logger)
        {
            _settingsService = settingsService;
            _snapshotService = snapshotService;
            _logger = logger;
        }
        
        protected override void DoWork()
        {
            var (_, enablePurgeSetting) = _settingsService.GetSetting("EnablePurge");
            var (_, purgeAgeMinutesSetting) = _settingsService.GetSetting("PurgeAgeMinutes");

            if (!bool.TryParse(enablePurgeSetting, out var enablePurge) || !int.TryParse(purgeAgeMinutesSetting, out var purgeAgeMinutes) || !enablePurge)
                return;

            var purgeCutoff = DateTime.UtcNow - TimeSpan.FromMinutes(purgeAgeMinutes);
            _logger.LogInformation($"Purging snapshot data before {purgeCutoff:MM/dd/yy H:mm}");

            var purgedCount = _snapshotService.DeleteOld(purgeCutoff);

            _logger.LogInformation($"Purged {purgedCount} snapshots.");
        }
    }
}