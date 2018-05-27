using AspNetCore.Identity.LiteDB.Models;

namespace MiningMonitor.Model
{
    public class MiningMonitorUser : ApplicationUser
    {
        public string CollectorName { get; set; }
        public bool? IsApproved { get; set; }
    }
}
