using System;
using System.Collections.Generic;

using AspNetCore.Identity.LiteDB;

using Microsoft.AspNetCore.Identity;

namespace MiningMonitor.Security.Identity
{

    public class MiningMonitorUser : IdentityUser<Guid>
    {
        public string CollectorName { get; set; }
        public bool? IsApproved { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public List<MiningMonitorClaim> Claims { get; set; } = new List<MiningMonitorClaim>();
    }
}