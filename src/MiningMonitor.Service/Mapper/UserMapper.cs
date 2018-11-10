using System;

using MiningMonitor.Common.Mapper;
using MiningMonitor.Model;
using MiningMonitor.Model.Identity;

namespace MiningMonitor.Service.Mapper
{
    public class UserMapper : 
        IMapper<(string currentUser, MiningMonitorUser user), UserListItem>, 
        IMapper<User, MiningMonitorUser>, 
        IMapper<MiningMonitorUser, Collector>, 
        IUpdateMapper<Collector, MiningMonitorUser>
    {
        public UserListItem Map((string currentUser, MiningMonitorUser user) source)
        {
            return new UserListItem
            {
                Username = source.user.UserName,
                Email = source.user.Email,
                IsCurrentUser = string.Equals(source.user.UserName, source.currentUser, StringComparison.OrdinalIgnoreCase)
            };
        }

        public MiningMonitorUser Map(User user)
        {
            return new MiningMonitorUser
            {
                UserName = user.Username,
                Email = user.Email
            };
        }

        public MiningMonitorUser Map(Collector collector)
        {
            return new MiningMonitorUser
            {
                Id = collector.Id,
                UserName = collector.Id.ToString(),
                CollectorName = collector.Name,
                IsApproved = collector.Approved
            };
        }

        public void Update(Collector collector, MiningMonitorUser user)
        {
            user.UserName = collector.Id.ToString();
            user.CollectorName = user.CollectorName;
            user.IsApproved = collector.Approved;
        }

        Collector IMapper<MiningMonitorUser, Collector>.Map(MiningMonitorUser user)
        {
            return new Collector
            {
                Id = user.Id,
                Name = user.CollectorName,
                Approved = user.IsApproved
            };
        }
    }
}