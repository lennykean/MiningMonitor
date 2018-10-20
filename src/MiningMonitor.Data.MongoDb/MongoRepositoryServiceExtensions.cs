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

            services.AddRepository<Snapshot, MongoDbRepository<Snapshot>>("snapshots");
            services.AddRepository<Miner, MongoDbRepository<Miner>>("miners");
            services.AddRepository<Setting, MongoDbRepository<Setting>>("settings");
            services.AddRepository<AlertDefinition, MongoDbRepository<AlertDefinition>>("alertdefinitions");
            services.AddRepository<Alert, MongoDbRepository<Alert>>("alerts");
            services.AddRepository<MiningMonitorUser, MongoDbRepository<MiningMonitorUser>>("users");
            services.AddRepository<MiningMonitorRole, MongoDbRepository<MiningMonitorRole>>("roles");

            return services;
        }

        private static void AddRepository<TDocument, TRepository>(this IServiceCollection services, string name) where TRepository : MongoDbRepository<TDocument>
        {
            services.AddTransient(service => service.GetService<IMongoDatabase>().GetCollection<TDocument>(name));
            services.AddTransient<IRepository<TDocument>, TRepository>();
        }
    }
}
