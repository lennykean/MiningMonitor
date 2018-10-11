using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace MiningMonitor.Data.LiteDb
{
    public class MongoDbRepository<T> : IRepository<T>
    {
        private readonly IMongoCollection<T> _collection;

        public MongoDbRepository(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public virtual async Task<IEnumerable<T>> FindAllAsync(CancellationToken token = default)
        {
            return await _collection.Find(_ => true).ToListAsync(token);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default)
        {
            return await _collection.Find(predicate).ToListAsync(token);
        }

        public virtual async Task<T> FindOneAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default)
        {
            return await _collection.Find(predicate).SingleOrDefaultAsync(token);
        }

        public virtual async Task<T> FindByIdAsync(Guid id, CancellationToken token = default)
        {
            return await _collection.Find(Builders<T>.Filter.Eq("_id", id)).SingleOrDefaultAsync(token);
        }

        public virtual async Task InsertAsync(T document, CancellationToken token = default)
        {
            await _collection.InsertOneAsync(document, cancellationToken: token);
        }

        public virtual async Task UpsertAsync(T document, CancellationToken token = default)
        {
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", GetId(document)), document, new UpdateOptions { IsUpsert = true }, token);
        }

        public virtual async Task<bool> UpdateAsync(T document, CancellationToken token = default)
        {
            return (await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", GetId(document)), document, cancellationToken: token)).IsAcknowledged;
        }

        public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            return (await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id), token)).IsAcknowledged;
        }

        public virtual async Task<int> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default)
        {
            return (int)(await _collection.DeleteManyAsync(predicate, token)).DeletedCount;
        }

        private static object GetId(T document)
        {
            return (
                from property in typeof(T).GetProperties()
                from attribute in property.CustomAttributes
                where attribute.AttributeType == typeof(BsonIdAttribute)
                select property.GetValue(document)).Single();
        }
    }
}
