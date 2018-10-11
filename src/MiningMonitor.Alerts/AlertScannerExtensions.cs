using Microsoft.Extensions.DependencyInjection;

using MiningMonitor.Alerts;
using MiningMonitor.Alerts.Scanners;

namespace MiningMonitor.Data.MongoDb
{
    public static class AlertScannerExtensions
    {
        public static IServiceCollection AddAlertScanners(this IServiceCollection services)
        {
            services.AddTransient<IScanFactory, ScanFactory>();
            services.AddTransient<IAlertScanner, HashrateScanner>();
            services.AddTransient<IAlertScanner, GpuHashrateThresholdScanner>();
            services.AddTransient<IAlertScanner, GpuTemperatureThresholdScanner>();
            services.AddTransient<IAlertScanner, GpuFanSpeedThresholdScanner>();
            services.AddTransient<IAlertScanner, ConnectivityScanner>();

            return services;
        }
    }
}
