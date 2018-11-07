using System;

using AspNetCore.ClaimsValueProvider;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MiningMonitor.BackgroundScheduler;
using MiningMonitor.Data.MongoDb;
using MiningMonitor.Security.Authorization;
using MiningMonitor.Security.Identity;
using MiningMonitor.Service;
using MiningMonitor.Web.Swagger;
using MiningMonitor.Workers.AlertScan;
using MiningMonitor.Workers.DataCollector;
using MiningMonitor.Workers.DataSynchronizer;
using MiningMonitor.Workers.Maintenance;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Swashbuckle.AspNetCore.Swagger;

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
            // Repository
            if (_configuration.GetValue<bool>("use_mongo"))
                services.AddMongoRepository(_configuration.GetConnectionString("miningmonitor"));
            else
                services.AddLiteDbRepository(Environment.ExpandEnvironmentVariables(_configuration.GetConnectionString("miningmonitor")));

            // Services
            services.AddMiningMonitorServices();

            // Alerts
            services.AddAlerts();

            // Background workers
            if (!_configuration.GetValue<bool>("disable_background_workers"))
            {
                services.AddBackgroundWorker<DataCollectorWorker>(_configuration.GetSection("Scheduler:DataCollector"));
                services.AddBackgroundWorker<DataSynchronizerWorker>(_configuration.GetSection("Scheduler:DataSynchronizer"));
                services.AddBackgroundWorker<MaintenanceWorker>(_configuration.GetSection("Scheduler:Maintenance"));
                services.AddBackgroundWorker<AlertScanWorker>(_configuration.GetSection("Scheduler:AlertScan"));
            }

            // Security
            services.AddIdentity<MiningMonitorUser, MiningMonitorRole>()
                .RegisterMiningMonitorStores()
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
            services
                .AddResponseCompression()
                .AddMvc(options => options.AddClaimsValueProvider())
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });
            services.AddSpaStaticFiles(configuration =>
            {
                if (Environment.GetEnvironmentVariable("ANGULAR_DEV_SERVER") == "true")
                    configuration.RootPath = "ClientApp";
                else 
                    configuration.RootPath = "ClientApp/dist";
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info {Title = "Mining Monitor API - V1", Version = "v1"});
                options.OperationFilter<ApiGatewayIntegrationFilter>();
                options.OperationFilter<FromClaimParameterFilter>();
                options.SchemaFilter<ReadonlyFilter>();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider service)
        {
            app.UseResponseCompression()
                .UseStaticFiles()
                .UseAuthentication()
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                })
                .UseSwagger();

            app.UseSpaStaticFiles();
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