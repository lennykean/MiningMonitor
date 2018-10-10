namespace MiningMonitor.Security.Authorization
{
    public class OwnResourceWhenEnabledRequirement : IWhenEnabledRequirement
    {
        public OwnResourceWhenEnabledRequirement(string routeValue)
        {
            RouteValue = routeValue;
        }

        public string RouteValue { get; }
    }
}