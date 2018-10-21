using System;

using Microsoft.Extensions.DependencyInjection;

using MiningMonitor.Data.LiteDb;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;
using MiningMonitor.Security.Identity;

using MongoDB.Driver;

namespace MiningMonitor.Data.MongoDb
{
    public static class MongoRepositoryServiceExtensions
    {
        public static IServiceCollection AddMongoRepository(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton(service => new MongoClient(connectionString));
            services.AddTransient(service => service.GetService<MongoClient>().GetDatabase("miningmonitor"));

            services.AddRepository<Snapshot, Guid, MongoDbRepository<Snapshot, Guid>>("snapshots");
            services.AddRepository<Miner, Guid, MongoDbRepository<Miner, Guid>>("miners");
            services.AddRepository<Setting, string, MongoDbRepository<Setting, string>>("settings");
            services.AddRepository<AlertDefinition, Guid, MongoDbRepository<AlertDefinition, Guid>>("alertdefinitions");
            services.AddRepository<Alert, Guid, MongoDbRepository<Alert, Guid>>("alerts");
            services.AddRepository<MiningMonitorUser, Guid, MongoDbRepository<MiningMonitorUser, Guid>>("users");
            services.AddRepository<MiningMonitorRole, Guid, MongoDbRepository<MiningMonitorRole, Guid>>("roles");

            return services;
        }

        private static void AddRepository<TDocument, TKey, TRepository>(this IServiceCollection services, string name) where TRepository : MongoDbRepository<TDocument, TKey>
        {
            services.AddTransient(service => service.GetService<IMongoDatabase>().GetCollection<TDocument>(name));
            services.AddTransient<IRepository<TDocument, TKey>, TRepository>();
        }
    }
}
