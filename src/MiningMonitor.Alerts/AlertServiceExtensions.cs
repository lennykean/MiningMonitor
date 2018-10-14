using Microsoft.Extensions.DependencyInjection;

using MiningMonitor.Alerts;
using MiningMonitor.Alerts.Scanners;
using MiningMonitor.Alerts.Triggers;

namespace MiningMonitor.Data.MongoDb
{
    public static class AlertServiceExtensions
    {
        public static IServiceCollection AddAlerts(this IServiceCollection services)
        {
            // Scanners
            services.AddTransient<IScanFactory, ScanFactory>();
            services.AddTransient<IAlertScanner, HashrateScanner>();
            services.AddTransient<IAlertScanner, GpuHashrateThresholdScanner>();
            services.AddTransient<IAlertScanner, GpuTemperatureThresholdScanner>();
            services.AddTransient<IAlertScanner, GpuFanSpeedThresholdScanner>();
            services.AddTransient<IAlertScanner, ConnectivityScanner>();

            // Triggers
            services.AddTransient<ITriggerProcessor, TriggerProcessor>();
            services.AddTransient<IAlertTrigger, DisableGpuAlertTrigger>();
            services.AddTransient<IAlertTrigger, RestartMinerAlertTrigger>();
            services.AddTransient<IAlertTrigger, WebHookAlertTrigger>();

            return services;
        }
    }
}
