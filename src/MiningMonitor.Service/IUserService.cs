using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync(CancellationToken token = default);
        Task<ModelStateDictionary> CreateUserAsync(User user, CancellationToken token = default);
    }
}