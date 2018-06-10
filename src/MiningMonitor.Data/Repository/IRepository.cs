using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MiningMonitor.Data
{
    public interface IRepository<T, in TKey>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(TKey id);
        Task AddAsync(T item);
        Task<int> BulkAddAsync(IEnumerable<T> items);
        Task<bool> UpdateAsync(T item);
        Task<bool> DeleteAsync(TKey id);
        Task<int> DeleteAsync(Expression<Func<T, bool>> predicate);
    }
}