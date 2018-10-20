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

            services.AddRepository<Snapshot, LiteDbSnapshotRepository>();
            services.AddRepository<Miner, LiteDbRepository<Miner>>();
            services.AddRepository<Setting, LiteDbRepository<Setting>>();
            services.AddRepository<AlertDefinition, LiteDbAlertDefinitionRepository>();
            services.AddRepository<Alert, LiteDbAlertRepository>();
            services.AddRepository<MiningMonitorUser, LiteDbRepository<MiningMonitorUser>>();
            services.AddRepository<MiningMonitorRole, LiteDbRepository<MiningMonitorRole>>();

            return services;
        }

        private static void AddRepository<TDocument, TRepository>(this IServiceCollection services) where TRepository : LiteDbRepository<TDocument>
        {
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<TDocument>());
            services.AddTransient<IRepository<TDocument>, TRepository>();
        }
    }
}
