using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using MiningMonitor.Model;
using MiningMonitor.Service.Mapper;

namespace MiningMonitor.Service
{
    public class CollectorService : ICollectorService
    {
        private readonly UserManager<MiningMonitorUser> _userManager;
        private readonly ILoginService _loginService;
        private readonly IMapper<MiningMonitorUser, Collector> _collectorMapper;
        private readonly IUpdateMapper<Collector, MiningMonitorUser> _userMapper;
        private readonly IMapper<IdentityResult, ModelStateDictionary> _resultMapper;

        public CollectorService(
            UserManager<MiningMonitorUser> userManager,
            ILoginService loginService,
            IMapper<MiningMonitorUser, Collector> collectorMapper,
            IUpdateMapper<Collector, MiningMonitorUser> userMapper,
            IMapper<IdentityResult, ModelStateDictionary> resultMapper)
        {
            _userManager = userManager;
            _loginService = loginService;
            _collectorMapper = collectorMapper;
            _userMapper = userMapper;
            _resultMapper = resultMapper;
        }

        public Task<IEnumerable<Collector>> GetAllAsync()
        {
            return Task.Run(() => _userManager.Users.Where(u => u.Roles.Contains("Collector")).AsEnumerable().Select(_collectorMapper.Map));
        }

        public async Task<Collector> Get(string collector)
        {
            return _collectorMapper.Map(await _userManager.FindByNameAsync(collector));
        }

        public async Task<(ModelStateDictionary modelState, RegistrationResponse registration)> CreateCollectorAsync(Collector collector)
        {
            collector.Id = Guid.NewGuid().ToString();
            collector.Approved = null;
            
            var identityUser = _userMapper.Map(collector);
            identityUser.Roles.Add("Collector");

            var result = _resultMapper.Map(await _userManager.CreateAsync(identityUser));
            
            return (result, result.IsValid ? new RegistrationResponse
            {
                Id = identityUser.UserName,
                Token = _loginService.CreateToken(identityUser.UserName)
            } : null);
        }

        public async Task<bool> UpdateAsync(Collector collector)
        {
            var user = await _userManager.FindByNameAsync(collector.Id);
            _userMapper.Update(collector, user);

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}