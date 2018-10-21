using System;

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

            services.AddRepository<Snapshot, Guid, LiteDbSnapshotRepository>();
            services.AddRepository<Miner, Guid, LiteDbRepository<Miner, Guid>>();
            services.AddRepository<Setting, string, LiteDbRepository<Setting, string>>();
            services.AddRepository<AlertDefinition, Guid, LiteDbAlertDefinitionRepository>();
            services.AddRepository<Alert, Guid, LiteDbAlertRepository>();
            services.AddRepository<MiningMonitorUser, Guid, LiteDbRepository<MiningMonitorUser, Guid>>();
            services.AddRepository<MiningMonitorRole, Guid, LiteDbRepository<MiningMonitorRole, Guid>>();

            return services;
        }

        private static void AddRepository<TDocument, TKey, TRepository>(this IServiceCollection services) where TRepository : LiteDbRepository<TDocument, TKey>
        {
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<TDocument>());
            services.AddTransient<IRepository<TDocument, TKey>, TRepository>();
        }
    }
}
