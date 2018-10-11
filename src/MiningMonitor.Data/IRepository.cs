using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MiningMonitor.Data
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> FindAllAsync(CancellationToken token = default);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default);
        Task<T> FindOneAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default);
        Task<T> FindByIdAsync(Guid id, CancellationToken token = default);
        Task InsertAsync(T document, CancellationToken token = default);
        Task UpsertAsync(T document, CancellationToken token = default);
        Task<bool> UpdateAsync(T document, CancellationToken token = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken token = default);
        Task<int> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default);
    }
}