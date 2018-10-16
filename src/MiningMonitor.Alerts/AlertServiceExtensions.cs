using Microsoft.Extensions.DependencyInjection;

using MiningMonitor.Alerts;
using MiningMonitor.Alerts.Action;
using MiningMonitor.Alerts.Scanners;

namespace MiningMonitor.Data.MongoDb
{
    public static class AlertServiceExtensions
    {
        public static IServiceCollection AddAlerts(this IServiceCollection services)
        {
            services.AddTransient<IAlertFactory, AlertFactory>();

            // Scanners
            services.AddTransient<IScanFactory, ScanFactory>();
            services.AddTransient<IAlertScanner, HashrateScanner>();
            services.AddTransient<IAlertScanner, GpuHashrateThresholdScanner>();
            services.AddTransient<IAlertScanner, GpuTemperatureThresholdScanner>();
            services.AddTransient<IAlertScanner, GpuFanSpeedThresholdScanner>();
            services.AddTransient<IAlertScanner, ConnectivityScanner>();

            // Actions
            services.AddTransient<IAlertActionExecutor, DisableGpuAlertActionExecutor>();
            services.AddTransient<IAlertActionExecutor, RestartMinerAlertActionExecutor>();
            services.AddTransient<IAlertActionExecutor, WebHookAlertActionExecutor>();

            return services;
        }
    }
}
