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
            var dataDir = new DirectoryInfo(Environment.ExpandEnvironmentVariables("%ProgramData%\\MiningMonitor"));
            dataDir.Create();

            var host = BuildWebHost(args);

            if (Debugger.IsAttached)
                host.Run();
            else
                host.RunAsService();
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
