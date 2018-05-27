using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiningMonitor.Data
{
    public interface IRepository<T, TKey>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(TKey id);
        Task AddAsync(T item);
        Task<int> BulkAddAsync(IEnumerable<T> items);
        Task<bool> UpdateAsync(T item);
        Task<bool> DeleteAsync(TKey id);
    }
}