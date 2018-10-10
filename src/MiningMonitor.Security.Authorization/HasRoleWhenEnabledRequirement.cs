namespace MiningMonitor.Security.Authorization
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