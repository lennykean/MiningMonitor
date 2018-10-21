using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using MiningMonitor.Common.Mapper;
using MiningMonitor.Model;
using MiningMonitor.Security.Identity;

namespace MiningMonitor.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<MiningMonitorUser> _userManager;
        private readonly IMapper<(string currentUser, MiningMonitorUser user), UserListItem> _userMapper;
        private readonly IMapper<User, MiningMonitorUser> _identityUserMapper;
        private readonly IMapper<IdentityResult, ModelStateDictionary> _resultMapper;

        public UserService(
            UserManager<MiningMonitorUser> userManager,
            IMapper<(string currentUser, MiningMonitorUser user), UserListItem> userMapper,
            IMapper<User, MiningMonitorUser> identityUserMapper,
            IMapper<IdentityResult, ModelStateDictionary> resultMapper)
        {
            _userManager = userManager;
            _userMapper = userMapper;
            _identityUserMapper = identityUserMapper;
            _resultMapper = resultMapper;
        }

        public Task<IEnumerable<UserListItem>> GetUsersAsync(string currentUser, CancellationToken token = default)
        {
            return Task.Run(() => 
                from user in _userManager.Users.AsEnumerable()
                where !user.Roles.Contains("Collector")
                select _userMapper.Map((currentUser, user)), token);
        }

        public async Task<ModelStateDictionary> CreateUserAsync(User user, CancellationToken token = default)
        {
            var identityUser = _identityUserMapper.Map(user);
            
            return _resultMapper.Map(await _userManager.CreateAsync(identityUser, user.Password));
        }

        public async Task<bool> DeleteAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return false;

            await _userManager.DeleteAsync(user);

            return true;
        }
    }
}