using LiteDB;

using Microsoft.Extensions.DependencyInjection;

using MiningMonitor.Data.LiteDb;
using MiningMonitor.Data.LiteDb.Serialization;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;
using MiningMonitor.Security.Identity;

namespace MiningMonitor.Data.MongoDb
{
    public static class LiteDbRepositoryServiceExtensions
    {
        public static IServiceCollection AddLiteDbRepository(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton(service => new LiteDatabase(connectionString, new MiningMonitorBsonMapper()));
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<Snapshot>());
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<Miner>());
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<Setting>());
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<AlertDefinition>());
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<Alert>());
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<MiningMonitorUser>());
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<MiningMonitorRole>());

            services.AddTransient<IRepository<Snapshot>, LiteDbSnapshotRepository>();
            services.AddTransient<IRepository<Miner>, LiteDbRepository<Miner>>();
            services.AddTransient<IRepository<Setting>, LiteDbRepository<Setting>>();
            services.AddTransient<IRepository<AlertDefinition>, LiteDbAlertDefinitionRepository>();
            services.AddTransient<IRepository<Alert>, LiteDbAlertRepository>();
            services.AddTransient<IRepository<MiningMonitorUser>, LiteDbRepository<MiningMonitorUser>>();
            services.AddTransient<IRepository<MiningMonitorRole>, LiteDbRepository<MiningMonitorRole>>();

            return services;
        }
    }
}
