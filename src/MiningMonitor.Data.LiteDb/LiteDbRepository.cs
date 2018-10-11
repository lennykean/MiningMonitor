using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using LiteDB;

namespace MiningMonitor.Data.LiteDb
{
    public class LiteDbRepository<T> : IRepository<T>
    {
        private readonly LiteCollection<T> _collection;

        public LiteDbRepository(LiteCollection<T> collection)
        {
            _collection = collection;
        }

        public virtual Task<IEnumerable<T>> FindAllAsync(CancellationToken token = default)
        {
            return Task.Run(() => _collection.FindAll(), token);
        }

        public virtual Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default)
        {
            return Task.Run(() => _collection.Find(predicate), token);
        }

        public virtual Task<T> FindOneAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default)
        {
            return Task.Run(() => _collection.FindOne(predicate), token);
        }

        public virtual Task<T> FindByIdAsync(Guid id, CancellationToken token = default)
        {
            return Task.Run(() => _collection.FindById(id), token);
        }

        public virtual Task InsertAsync(T document, CancellationToken token = default)
        {
            return Task.Run(() => _collection.Insert(document), token);
        }

        public virtual Task<bool> UpdateAsync(T document, CancellationToken token = default)
        {
            return Task.Run(() => _collection.Update(document), token);
        }

        public virtual Task UpsertAsync(T document, CancellationToken token = default)
        {
            return Task.Run(() => _collection.Upsert(document), token);
        }

        public virtual Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            return Task.Run(() => _collection.Delete(id), token);
        }

        public virtual Task<int> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default)
        {
            return Task.Run(() => _collection.Delete(predicate), token);
        }
    }
}
