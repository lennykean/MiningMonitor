namespace MiningMonitor.Web.Security
{
    public class HasRoleWhenEnabledRequirement : IWhenEnabledRequirement
    {
        public HasRoleWhenEnabledRequirement(string role)
        {
            Role = role;
        }

        public string Role { get; }
    }
}