using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

using MiningMonitor.Service;

namespace MiningMonitor.Web.Security
{
    public class MiningMonitorAuthorizationHandler : AuthorizationHandler<IWhenEnabledRequirement>
    {
        private readonly ISettingsService _settingsService;

        public MiningMonitorAuthorizationHandler(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IWhenEnabledRequirement requirement)
        {
            var (_, securityEnabled) = await _settingsService.GetSettingAsync("EnableSecurity");

            if (securityEnabled != "true")
            {
                context.Succeed(requirement);
                return;
            }
            var mvcContext = context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext;

            switch (requirement)
            {
                case AuthenticatedWhenEnabledRequirement _:
                    if (context.User.Identity.IsAuthenticated)
                        context.Succeed(requirement);
                    return;
                case HasRoleWhenEnabledRequirement hasRoleRequirement:
                    if (context.User.IsInRole(hasRoleRequirement.Role))
                        context.Succeed(requirement);
                    return;
                case OwnResourceWhenEnabledRequirement ownResourceRequirement:
                    if (mvcContext?.RouteData.Values[ownResourceRequirement.RouteValue]?.ToString() == context.User.Identity.Name)
                        context.Succeed(requirement);
                    else
                        context.Fail();
                    return;
            }
        }
    }
}