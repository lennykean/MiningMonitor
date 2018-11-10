using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using MiningMonitor.Model.Identity;

namespace MiningMonitor.Security.Identity
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