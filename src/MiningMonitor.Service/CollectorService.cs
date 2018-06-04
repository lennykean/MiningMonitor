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
        private readonly IMinerService _minerService;
        private readonly ISnapshotService _snapshotService;
        private readonly IMapper<MiningMonitorUser, Collector> _collectorMapper;
        private readonly IUpdateMapper<Collector, MiningMonitorUser> _userMapper;
        private readonly IMapper<IdentityResult, ModelStateDictionary> _resultMapper;
        
        public CollectorService(
            UserManager<MiningMonitorUser> userManager,
            ILoginService loginService,
            IMinerService minerService,
            ISnapshotService snapshotService,
            IMapper<MiningMonitorUser, Collector> collectorMapper,
            IUpdateMapper<Collector, MiningMonitorUser> userMapper,
            IMapper<IdentityResult, ModelStateDictionary> resultMapper)
        {
            _userManager = userManager;
            _loginService = loginService;
            _minerService = minerService;
            _snapshotService = snapshotService;
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
                Token = _loginService.LoginCollector(identityUser.UserName)
            } : null);
        }

        public async Task<bool> UpdateAsync(Collector collector)
        {
            var user = await _userManager.FindByNameAsync(collector.Id);
            _userMapper.Update(collector, user);

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(string collectorId)
        {
            var collector = (
                from u in _userManager.Users
                where u.Roles.Contains("Collector") && u.UserName == collectorId
                select u).FirstOrDefault();

            if (collector == null)
                return false;

            await _minerService.DeleteByCollectorAsync(collectorId);
            await _userManager.DeleteAsync(collector);

            return true;
        }

        public async Task<bool> MinerSyncAsync(string collector, Miner miner)
        {
            var existing = await _minerService.GetByIdAsync(miner.Id);
            if (existing != null)
            {
                if (existing.CollectorId != collector)
                    return false;

                miner.CollectorId = collector;
                await _minerService.UpdateAsync(miner);
            }
            else
            {
                miner.CollectorId = collector;
                await _minerService.AddAsync(miner);
            }
            return true;
        }

        public async Task<bool> SnapshotSyncAsync(string collector, Guid minerId, Snapshot snapshot)
        {
            var miner = await _minerService.GetByIdAsync(minerId);
            if (miner?.CollectorId != collector)
                return false;

            snapshot.MinerId = minerId;
            await _snapshotService.AddAsync(snapshot);

            return true;
        }
    }
}