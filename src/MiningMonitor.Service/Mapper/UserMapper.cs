using MiningMonitor.Common.Mapper;
using MiningMonitor.Model;

namespace MiningMonitor.Service.Mapper
{
    public class UserMapper : IMapper<MiningMonitorUser, User>, IMapper<User, MiningMonitorUser>, IMapper<MiningMonitorUser, Collector>, IUpdateMapper<Collector, MiningMonitorUser>
    {
        public User Map(MiningMonitorUser identityUser)
        {
            return new User
            {
                Username = identityUser.UserName,
                Email = identityUser.Email?.Address
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
                UserName = collector.Id,
                CollectorName = collector.Name,
                IsApproved = collector.Approved,
                Email = ""
            };
        }

        public void Update(Collector collector, MiningMonitorUser user)
        {
            user.UserName = collector.Id;
            user.CollectorName = user.CollectorName;
            user.IsApproved = collector.Approved;
        }

        Collector IMapper<MiningMonitorUser, Collector>.Map(MiningMonitorUser user)
        {
            return new Collector
            {
                Id = user.UserName,
                Name = user.CollectorName,
                Approved = user.IsApproved
            };
        }
    }
}