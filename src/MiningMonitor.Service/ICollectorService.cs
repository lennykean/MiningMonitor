using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public interface ICollectorService
    {
        Task<IEnumerable<Collector>> GetAllAsync();
        Task<Collector> Get(string collector);
        Task<(ModelStateDictionary modelState, RegistrationResponse registration)> CreateCollectorAsync(Collector collector);
        Task<bool> UpdateAsync(Collector collector);
    }
}