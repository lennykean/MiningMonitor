using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using MiningMonitor.Model;
using MiningMonitor.Service.Mapper;

namespace MiningMonitor.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<MiningMonitorUser> _userManager;
        private readonly IMapper<MiningMonitorUser, User> _userMapper;
        private readonly IMapper<User, MiningMonitorUser> _identityUserMapper;
        private readonly IMapper<IdentityResult, ModelStateDictionary> _resultMapper;

        public UserService(
            UserManager<MiningMonitorUser> userManager,
            IMapper<MiningMonitorUser, User> userMapper,
            IMapper<User, MiningMonitorUser> identityUserMapper,
            IMapper<IdentityResult, ModelStateDictionary> resultMapper)
        {
            _userManager = userManager;
            _userMapper = userMapper;
            _identityUserMapper = identityUserMapper;
            _resultMapper = resultMapper;
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            return Task.Run(() => _userManager.Users.AsEnumerable().Where(t => !t.Roles.Contains("Collector")).Select(_userMapper.Map));
        }

        public async Task<ModelStateDictionary> CreateUserAsync(User user)
        {
            var identityUser = _identityUserMapper.Map(user);
            
            return _resultMapper.Map(await _userManager.CreateAsync(identityUser, user.Password));
        }
    }
}