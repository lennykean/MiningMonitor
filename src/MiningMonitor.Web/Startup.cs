using System;
using System.Net.Http;

using AspNetCore.ClaimsValueProvider;
using AspNetCore.Identity.LiteDB;
using AspNetCore.Identity.LiteDB.Data;

using LiteDB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MiningMonitor.BackgroundWorker.DataCollector;
using MiningMonitor.BackgroundWorker.Maintenance;
using MiningMonitor.BackgroundWorker.Scheduler;
using MiningMonitor.Data;
using MiningMonitor.Data.Repository;
using MiningMonitor.Model;
using MiningMonitor.Service;
using MiningMonitor.Service.Mapper;
using MiningMonitor.Web.Configuration;
using MiningMonitor.Web.Security;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using IdentityRole = AspNetCore.Identity.LiteDB.IdentityRole;

namespace MiningMonitor.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            // Database
            services.AddSingleton(service => new LiteDatabase(_configuration.GetConnectionString("miningmonitor")));
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<Snapshot>());
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<Miner>());
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<Setting>());

            // Repository
            services.AddTransient<IMinerRepository, MinerRepository>();
            services.AddTransient<ISnapshotRepository, SnapshotRepository>();
            services.AddTransient<ISettingRepository, SettingRepository>();

            // Mappers
            services.AddTransient<IMapper<MiningMonitorUser, User>, UserMapper>();
            services.AddTransient<IMapper<User, MiningMonitorUser>, UserMapper>();
            services.AddTransient<IMapper<MiningMonitorUser, Collector>, UserMapper>();
            services.AddTransient<IUpdateMapper<Collector, MiningMonitorUser>, UserMapper>();
            services.AddTransient<IMapper<IdentityResult, ModelStateDictionary>, IdentityResultMapper>();

            // Services
            services.AddTransient<Func<HttpClient>>(s => () => new HttpClient());
            services.AddTransient<ICollectorService, CollectorService>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IMinerService, MinerService>();
            services.AddTransient<IServerService, ServerService>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ISnapshotService, SnapshotService>();
            services.AddTransient<IUserService, UserService>();

            // API Client
            services.AddTransient<IRemoteManagementClientFactory, RemoteManagementClientFactory>();

            // Background workers
            services.AddSingleton<IHostedService, BackgroundScheduler<SnapshotDataCollector, SnapshotDataCollectorSchedule>>();
            services.AddSingleton<IHostedService, BackgroundScheduler<DataSyncronizer, DataSyncronizerSchedule>>();
            services.AddSingleton<IHostedService, BackgroundScheduler<Purge, PurgeSchedule>>();
            services.ConfigurePOCO<SnapshotDataCollectorSchedule>(_configuration.GetSection("Scheduler:SnapshotDataCollector"));
            services.ConfigurePOCO<DataSyncronizerSchedule>(_configuration.GetSection("Scheduler:DataSyncronizer"));
            services.ConfigurePOCO<PurgeSchedule>(_configuration.GetSection("Scheduler:Purge"));
            services.AddTransient<SnapshotDataCollector>();
            services.AddTransient<DataSyncronizer>();
            services.AddTransient<Purge>();

            // Security
            services.AddSingleton(service => new LiteDbContext(service.GetService<Microsoft.AspNetCore.Hosting.IHostingEnvironment>())
            {
                LiteDatabase = service.GetService<LiteDatabase>()
            });
            services.AddIdentity<MiningMonitorUser, IdentityRole>()
                .AddUserStore<MiningMonitorUserStore<MiningMonitorUser>>()
                .AddRoleStore<LiteDbRoleStore<IdentityRole>>()
                .AddDefaultTokenProviders();
            services
                .AddAuthentication(config =>
                {
                    config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters.ValidateIssuer = false;
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.TokenValidationParameters.RequireExpirationTime = false;
                    options.TokenValidationParameters.RequireSignedTokens = false;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Basic", policy => policy.AddRequirements(
                    new AuthenticatedWhenEnabledRequirement()));
                options.AddPolicy("Collector", policy => policy.AddRequirements(
                    new HasRoleWhenEnabledRequirement("Collector"), 
                    new OwnResourceWhenEnabledRequirement("collector")));
            });
            services.AddTransient<IAuthorizationHandler, MiningMonitorAuthorizationHandler>();

            // MVC
            services.AddOptions()
                .AddMvc(options =>
                {
                    options.AddClaimsValueProvider();
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider service)
        {
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}