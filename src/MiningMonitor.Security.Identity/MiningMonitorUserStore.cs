using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using MiningMonitor.Data;
using MiningMonitor.Model.Identity;

namespace MiningMonitor.Security.Identity
{
    public class MiningMonitorUserStore :
        IUserPasswordStore<MiningMonitorUser>,
        IUserRoleStore<MiningMonitorUser>,
        IUserSecurityStampStore<MiningMonitorUser>,
        IUserEmailStore<MiningMonitorUser>,
        IUserClaimStore<MiningMonitorUser>,
        IUserLockoutStore<MiningMonitorUser>,
        IQueryableUserStore<MiningMonitorUser>
    {
        private readonly IRepository<MiningMonitorUser, Guid> _repository;
        private readonly ILogger<MiningMonitorUserStore> _logger;

        public MiningMonitorUserStore(IRepository<MiningMonitorUser, Guid> repository, ILogger<MiningMonitorUserStore> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
        }

        #endregion

        #region Implementation of IUserStore<MiningMonitorUser>

        public Task<string> GetUserIdAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(MiningMonitorUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync(MiningMonitorUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.InsertAsync(user, cancellationToken);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> UpdateAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            try
            {
                if (!await _repository.UpdateAsync(user, cancellationToken))
                    return IdentityResult.Failed();
                
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> DeleteAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            try
            {
                if (!await _repository.DeleteAsync(user.Id, cancellationToken))
                    return IdentityResult.Failed();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return IdentityResult.Failed();
            }
        }

        public async Task<MiningMonitorUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _repository.FindByIdAsync(Guid.Parse(userId), cancellationToken);
        }

        public async Task<MiningMonitorUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await _repository.FindOneAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);
        }

        #endregion

        #region Implementation of IUserPasswordStore<MiningMonitorUser>

        public Task SetPasswordHashAsync(MiningMonitorUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        #endregion

        #region Implementation of IUserRoleStore<MiningMonitorUser>

        public Task AddToRoleAsync(MiningMonitorUser user, string roleName, CancellationToken cancellationToken)
        {
            user.Roles.Add(roleName);
            return Task.CompletedTask;
        }

        public Task RemoveFromRoleAsync(MiningMonitorUser user, string roleName, CancellationToken cancellationToken)
        {
            user.Roles.Remove(roleName);
            return Task.CompletedTask;
        }

        public Task<IList<string>> GetRolesAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult<IList<string>>(user.Roles);
        }

        public Task<bool> IsInRoleAsync(MiningMonitorUser user, string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Roles.Contains(roleName));
        }

        public async Task<IList<MiningMonitorUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return (await _repository.FindAsync(u => u.Roles.Contains(roleName), cancellationToken)).ToList();
        }

        #endregion
        
        #region Implementation of IUserSecurityStampStore<MiningMonitorUser>

        public Task SetSecurityStampAsync(MiningMonitorUser user, string stamp, CancellationToken cancellationToken)
        {
            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }

        public Task<string> GetSecurityStampAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        #endregion

        #region Implementation of IUserEmailStore<MiningMonitorUser>

        public Task SetEmailAsync(MiningMonitorUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(MiningMonitorUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public async Task<MiningMonitorUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await _repository.FindOneAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
        }

        public Task<string> GetNormalizedEmailAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(MiningMonitorUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        #endregion

        #region Implementation of IUserClaimStore<MiningMonitorUser>

        public Task<IList<Claim>> GetClaimsAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult<IList<Claim>>(user.Claims.Select(c => new Claim(c.Type, c.Value)).ToList());
        }

        public Task AddClaimsAsync(MiningMonitorUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            user.Claims.AddRange(claims.Select(c => new MiningMonitorClaim
            {
                Type = c.Type,
                Value = c.Value
            }));
            return Task.CompletedTask;
        }

        public Task ReplaceClaimAsync(MiningMonitorUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            var existingClaim = user.Claims.FirstOrDefault(c => c.Type == claim.Type && c.Value == claim.Value);
            if (existingClaim != null)
                user.Claims.Remove(existingClaim);

            user.Claims.Add(new MiningMonitorClaim
            {
                Type = newClaim.Type,
                Value = newClaim.Value
            });
            return Task.CompletedTask;
        }

        public Task RemoveClaimsAsync(MiningMonitorUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            user.Claims.RemoveAll(c => claims.Any(cr => cr.Value == c.Value && cr.Type == c.Type));
            return Task.CompletedTask;
        }

        public async Task<IList<MiningMonitorUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            return (await _repository.FindAsync(u => u.Claims.Any(c => c.Type == claim.Type && c.Value == claim.Value), cancellationToken)).ToList();
        }

        #endregion

        #region Implementation of IUserLockoutStore<MiningMonitorUser>

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnd);
        }

        public Task SetLockoutEndDateAsync(MiningMonitorUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEnd = lockoutEnd?.UtcDateTime;
            return Task.CompletedTask;
        }

        public Task<int> IncrementAccessFailedCountAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(++user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount = 0;
            return Task.CompletedTask;
        }

        public Task<int> GetAccessFailedCountAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(MiningMonitorUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(MiningMonitorUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;
            return Task.CompletedTask;
        }

        #endregion

        #region Implementation of IQueryableUserStore<MiningMonitorUser>

        public IQueryable<MiningMonitorUser> Users => _repository.FindAllAsync().Result.AsQueryable();

        #endregion
    }
}