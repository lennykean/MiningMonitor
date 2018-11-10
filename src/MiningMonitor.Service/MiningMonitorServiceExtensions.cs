using System;
using System.Net.Http;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

using MiningMonitor.Common.Mapper;
using MiningMonitor.Model;
using MiningMonitor.Model.Identity;
using MiningMonitor.Security.Identity;
using MiningMonitor.Service.Mapper;

namespace MiningMonitor.Service
{
    public static class MiningMonitorServiceExtensions
    {
        public static IServiceCollection AddMiningMonitorServices(this IServiceCollection services)
        {
            // Services
            services.AddTransient<Func<HttpClient>>(s => () => new HttpClient());
            services.AddTransient<ICollectorService, CollectorService>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IMinerService, MinerService>();
            services.AddTransient<IServerService, ServerService>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ISnapshotService, SnapshotService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAlertDefinitionService, AlertDefinitionService>();
            services.AddTransient<IAlertService, AlertService>();

            // Mappers
            services.AddTransient<IMapper<(string currentUser, MiningMonitorUser user), UserListItem>, UserMapper>();
            services.AddTransient<IMapper<User, MiningMonitorUser>, UserMapper>();
            services.AddTransient<IMapper<MiningMonitorUser, Collector>, UserMapper>();
            services.AddTransient<IUpdateMapper<Collector, MiningMonitorUser>, UserMapper>();
            services.AddTransient<IMapper<IdentityResult, ModelStateDictionary>, IdentityResultMapper>();

            // Remote Management Client
            services.AddTransient<IRemoteManagementClientFactory, RemoteManagementClientFactory>();

            return services;
        }
    }
}
