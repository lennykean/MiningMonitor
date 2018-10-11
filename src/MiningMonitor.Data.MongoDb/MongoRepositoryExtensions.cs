using Microsoft.Extensions.DependencyInjection;

using MiningMonitor.Data.LiteDb;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;
using MiningMonitor.Security.Identity;

using MongoDB.Driver;

namespace MiningMonitor.Data.MongoDb
{
    public static class MongoRepositoryExtensions
    {
        public static IServiceCollection AddMongoRepository(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton(service => new MongoClient(connectionString));
            services.AddTransient(service => service.GetService<MongoClient>().GetDatabase("miningmonitor"));
            services.AddTransient(service => service.GetService<IMongoDatabase>().GetCollection<Snapshot>("snapshots"));
            services.AddTransient(service => service.GetService<IMongoDatabase>().GetCollection<Miner>("miners"));
            services.AddTransient(service => service.GetService<IMongoDatabase>().GetCollection<Setting>("settings"));
            services.AddTransient(service => service.GetService<IMongoDatabase>().GetCollection<AlertDefinition>("alertdefinitions"));
            services.AddTransient(service => service.GetService<IMongoDatabase>().GetCollection<Alert>("alerts"));
            services.AddTransient(service => service.GetService<IMongoDatabase>().GetCollection<MiningMonitorUser>("users"));
            services.AddTransient(service => service.GetService<IMongoDatabase>().GetCollection<MiningMonitorRole>("roles"));

            services.AddTransient<IRepository<Snapshot>, MongoDbRepository<Snapshot>>();
            services.AddTransient<IRepository<Miner>, MongoDbRepository<Miner>>();
            services.AddTransient<IRepository<Setting>, MongoDbRepository<Setting>>();
            services.AddTransient<IRepository<AlertDefinition>, MongoDbRepository<AlertDefinition>>();
            services.AddTransient<IRepository<Alert>, MongoDbRepository<Alert>>();
            services.AddTransient<IRepository<MiningMonitorUser>, MongoDbRepository<MiningMonitorUser>>();
            services.AddTransient<IRepository<MiningMonitorRole>, MongoDbRepository<MiningMonitorRole>>();

            return services;
        }
    }
}
