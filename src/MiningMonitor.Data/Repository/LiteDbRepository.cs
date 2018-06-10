using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using LiteDB;

namespace MiningMonitor.Data
{
    public class LiteDbRepository<T, TKey> : IRepository<T, TKey>
    {
        public LiteDbRepository(LiteCollection<T> dbCollection)
        {
            DbCollection = dbCollection;
        }

        protected LiteCollection<T> DbCollection { get; }

        public Task<IEnumerable<T>> GetAllAsync() => Task.Run(() => DbCollection.FindAll());

        public Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> predicate) => Task.Run(() => DbCollection.Find(predicate));

        public Task<T> GetByIdAsync(TKey id) => Task.Run(() => DbCollection.FindById(new BsonValue(id)));

        public Task AddAsync(T item) => Task.Run(() => DbCollection.Insert(item));

        public Task<int> BulkAddAsync(IEnumerable<T> items) => Task.Run(() => DbCollection.InsertBulk(items));

        public Task<bool> UpdateAsync(T item) => Task.Run(() => DbCollection.Update(item));

        public Task<bool> DeleteAsync(TKey id) => Task.Run(() => DbCollection.Delete(new BsonValue(id)));

        public Task<int> DeleteAsync(Expression<Func<T, bool>> predicate) => Task.Run(() => DbCollection.Delete(predicate));
    }
}