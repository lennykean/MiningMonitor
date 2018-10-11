using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;

using MiningMonitor.Security.Identity;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MongoIdentityBuilderExtensions
    {
        public static IdentityBuilder RegisterMiningMonitorStores(this IdentityBuilder builder)
        {
            builder.Services.AddSingleton<IUserStore<MiningMonitorUser>, MiningMonitorUserStore>();
            builder.Services.AddSingleton<IRoleStore<MiningMonitorRole>, MiningMonitorRoleStore>();

            return builder;
        }
    }
}