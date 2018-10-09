using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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

        public virtual IEnumerable<T> FindAll() => _collection.FindAll();

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _collection.Find(predicate);

        public virtual T FindOne(Expression<Func<T, bool>> predicate) => _collection.FindOne(predicate);

        public virtual T FindById(Guid id) => _collection.FindById(id);

        public virtual void Insert(T document) => _collection.Insert(document);

        public virtual bool Update(T document) => _collection.Update(document);

        public virtual void Upsert(T document) => _collection.Upsert(document);

        public virtual bool Delete(Guid id) => _collection.Delete(id);

        public virtual int Delete(Expression<Func<T, bool>> predicate) => _collection.Delete(predicate);
    }
}
