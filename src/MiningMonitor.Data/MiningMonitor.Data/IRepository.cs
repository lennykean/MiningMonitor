using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MiningMonitor.Data
{
    public interface IRepository<T>
    {
        IEnumerable<T> FindAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        T FindOne(Expression<Func<T, bool>> predicate);
        T FindById(Guid id);
        void Insert(T document);
        void Upsert(T document);
        bool Update(T document);
        bool Delete(Guid id);
        int Delete(Expression<Func<T, bool>> predicate);
    }
}