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

using MiningMonitor.Alerts;
using MiningMonitor.Alerts.Scanners;
using MiningMonitor.BackgroundScheduler;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;
using MiningMonitor.Model.Serialization;
using MiningMonitor.Service;
using MiningMonitor.Service.Mapper;
using MiningMonitor.Web.Configuration;
using MiningMonitor.Web.Security;
using MiningMonitor.Workers.AlertScan;
using MiningMonitor.Workers.DataCollector;
using MiningMonitor.Workers.DataSynchronizer;
using MiningMonitor.Workers.Maintenance;

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
            services.AddResponseCompression();
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
            services.AddSingleton(service => new LiteDatabase(_configuration.GetConnectionString("miningmonitor"), new MiningMonitorBsonMapper()));
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<Snapshot>());
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<Miner>());
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<Setting>());
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<AlertDefinition>());
            services.AddTransient(service => service.GetService<LiteDatabase>().GetCollection<Alert>());

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
            services.AddTransient<IAlertDefinitionService, AlertDefinitionService>();
            services.AddTransient<IAlertService, AlertService>();

            // API Client
            services.AddTransient<IRemoteManagementClientFactory, RemoteManagementClientFactory>();

            // Background workers
            services.AddSingleton<IHostedService, BackgroundScheduler<DataCollectorWorker, DataCollectorSchedule>>();
            services.AddSingleton<IHostedService, BackgroundScheduler<DataSynchronizerWorker, DataSynchronizerSchedule>>();
            services.AddSingleton<IHostedService, BackgroundScheduler<MaintenanceWorker, MaintenanceSchedule>>();
            services.AddSingleton<IHostedService, BackgroundScheduler<AlertScanWorker, AlertScanSchedule>>();
            services.ConfigurePOCO<DataCollectorSchedule>(_configuration.GetSection("Scheduler:DataCollector"));
            services.ConfigurePOCO<DataSynchronizerSchedule>(_configuration.GetSection("Scheduler:DataSynchronizer"));
            services.ConfigurePOCO<MaintenanceSchedule>(_configuration.GetSection("Scheduler:Maintenance"));
            services.ConfigurePOCO<AlertScanSchedule>(_configuration.GetSection("Scheduler:AlertScan"));
            services.AddTransient<DataCollectorWorker>();
            services.AddTransient<DataSynchronizerWorker>();
            services.AddTransient<MaintenanceWorker>();
            services.AddTransient<AlertScanWorker>();

            // Alerts
            services.AddTransient<IScanFactory, ScanFactory>();
            services.AddTransient<IAlertScanner, HashrateScanner>();
            services.AddTransient<IAlertScanner, GpuHashrateThresholdScanner>();
            services.AddTransient<IAlertScanner, GpuTemperatureThresholdScanner>();
            services.AddTransient<IAlertScanner, GpuFanSpeedThresholdScanner>();
            services.AddTransient<IAlertScanner, ConnectivityScanner>();

            // Security
            services.AddSingleton(service => new LiteDbContext(service.GetService<Microsoft.AspNetCore.Hosting.IHostingEnvironment>())
            {
                LiteDatabase = service.GetService<LiteDatabase>()
            });
            services.AddIdentity<MiningMonitorUser, IdentityRole>()
                .AddUserStore<QueryableLiteDbUserStore<MiningMonitorUser>>()
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
                if (Environment.GetEnvironmentVariable("ANGULAR_DEV_SERVER") == "true")
                    configuration.RootPath = "ClientApp";
                else 
                    configuration.RootPath = "ClientApp/dist";
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider service)
        {
            app.UseResponseCompression();
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

                if (Environment.GetEnvironmentVariable("ANGULAR_DEV_SERVER") == "true")
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}