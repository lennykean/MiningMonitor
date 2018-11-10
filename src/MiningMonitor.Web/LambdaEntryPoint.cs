using Amazon.Lambda.AspNetCoreServer;

using Microsoft.AspNetCore.Hosting;

namespace MiningMonitor.Web
{
    public class LambdaEntryPoint : APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder.UseStartup<Startup>();
        }
    }
}
