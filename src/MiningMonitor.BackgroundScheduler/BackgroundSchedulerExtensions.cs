using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MiningMonitor.Web.Configuration;
using MiningMonitor.Workers;

namespace MiningMonitor.BackgroundScheduler
{
    public static class BackgroundSchedulerExtensions
    {
        public static IServiceCollection AddBackgroundWorker<TWorker, TSchedule>(this IServiceCollection services, IConfigurationSection configSection)
            where TWorker : class, IWorker
            where TSchedule : class, ISchedule, new()
        {
            services.ConfigurePoco<TSchedule>(configSection);
            services.AddTransient<TWorker>();
            services.AddSingleton<IHostedService, BackgroundScheduler<TWorker, TSchedule>>();

            return services;
        }
    }
}
