using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Logging;

namespace MiningMonitor.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dataDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "MiningMonitor"));
            dataDir.Create();

            BuildWebHost(args).RunAsService();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);

            return WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((context, config) => config.AddFile(context.Configuration.GetSection("Logging")))
                .UseContentRoot(pathToContentRoot)
                .UseStartup<Startup>()
                .Build();
        }
    }
}
