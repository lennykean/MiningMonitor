using System;
using System.Collections.Generic;

using Mongo = MongoDB.Bson.Serialization.Attributes;

namespace MiningMonitor.Model.Identity
{
    public class MiningMonitorUser
    {
        [LiteDB.BsonId(autoId: false), Mongo.BsonId]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string SecurityStamp { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string NormalizedEmail { get; set; }
        public string PasswordHash { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public string CollectorName { get; set; }
        public bool? IsApproved { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public List<MiningMonitorClaim> Claims { get; set; } = new List<MiningMonitorClaim>();
    }
}