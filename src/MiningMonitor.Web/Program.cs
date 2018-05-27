using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MiningMonitor.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dataDir = new DirectoryInfo("data");
            dataDir.Create();

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
