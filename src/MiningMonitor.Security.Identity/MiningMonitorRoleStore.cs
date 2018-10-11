using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MiningMonitor.Data;
using MiningMonitor.Security.Identity;

namespace Microsoft.AspNetCore.Identity.MongoDB
{
    public class MiningMonitorRoleStore : IRoleStore<MiningMonitorRole>
    {
        private readonly IRepository<MiningMonitorRole> _repository;
        private readonly ILogger<MiningMonitorRoleStore> _logger;

        public MiningMonitorRoleStore(IRepository<MiningMonitorRole> repository, ILogger<MiningMonitorRoleStore> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        
        #region Implementation of IDisposable

        public void Dispose()
        {
        }

        #endregion

        #region Implementation of IRoleStore<MiningMonitorRole>

        public async Task<IdentityResult> CreateAsync(MiningMonitorRole role, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.InsertAsync(role, cancellationToken);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> UpdateAsync(MiningMonitorRole role, CancellationToken cancellationToken)
        {
            try
            {
                if (!await _repository.UpdateAsync(role, cancellationToken))
                    return IdentityResult.Failed();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> DeleteAsync(MiningMonitorRole role, CancellationToken cancellationToken)
        {
            try
            {
                if (!await _repository.DeleteAsync(role.Id, cancellationToken))
                    return IdentityResult.Failed();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return IdentityResult.Failed();
            }
        }

        public Task<string> GetRoleIdAsync(MiningMonitorRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(MiningMonitorRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(MiningMonitorRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(MiningMonitorRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(MiningMonitorRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public async Task<MiningMonitorRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await _repository.FindByIdAsync(Guid.Parse(roleId), cancellationToken);
        }

        public async Task<MiningMonitorRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await _repository.FindOneAsync(u => u.NormalizedName == normalizedRoleName, cancellationToken);
        }

        #endregion
    }
}