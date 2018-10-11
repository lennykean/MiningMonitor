using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MiningMonitor.Data
{
    public interface IRepository<T>
    {
        //IEnumerable<T> FindAll();
        //IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        //T FindOne(Expression<Func<T, bool>> predicate);
        //T FindById(Guid id);
        //void Insert(T document);
        //void Upsert(T document);
        //bool Update(T document);
        //bool Delete(Guid id);
        //int Delete(Expression<Func<T, bool>> predicate);

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