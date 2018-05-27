using Microsoft.AspNetCore.Authorization;

namespace MiningMonitor.Web.Security
{
    public interface IWhenEnabledRequirement : IAuthorizationRequirement
    {
    }
}