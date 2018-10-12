using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MiningMonitor.Data.MongoDb;
using MiningMonitor.Service;

namespace MiningMonitor.Workers.Maintenance
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets(Assembly.GetEntryAssembly(), optional: true)
                .Build();

            using (var serviceProvider = new ServiceCollection()
                .AddLogging(config => config.AddConsole())
                .AddMongoRepository(configuration.GetConnectionString("miningmonitor"))
                .AddMiningMonitorServices()
                .AddSingleton<MaintenanceWorker>()
                .BuildServiceProvider())
            {

                var worker = serviceProvider.GetService<MaintenanceWorker>();

                await worker.DoWorkAsync(CancellationToken.None);
            }
        }
    }
}
