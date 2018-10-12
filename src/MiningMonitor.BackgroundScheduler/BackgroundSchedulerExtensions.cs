using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MiningMonitor.Workers;

namespace MiningMonitor.BackgroundScheduler
{
    public static class BackgroundSchedulerExtensions
    {
        public static IServiceCollection AddBackgroundWorker<TWorker>(this IServiceCollection services, IConfiguration scheduleConfig)
            where TWorker : class, IWorker
        {
            var schedule = new Schedule<TWorker>();

            scheduleConfig.Bind(schedule);
            services.AddSingleton(schedule);

            services.AddTransient<TWorker>();
            services.AddSingleton<IHostedService, BackgroundScheduler<TWorker>>();

            return services;
        }
    }
}
