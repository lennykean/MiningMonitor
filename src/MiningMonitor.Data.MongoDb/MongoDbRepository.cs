using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

        public virtual IEnumerable<T> FindAll() => _collection.Find(_ => true).ToList();

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _collection.Find(predicate).ToList();

        public virtual T FindOne(Expression<Func<T, bool>> predicate) => _collection.Find(predicate).SingleOrDefault();

        public virtual T FindById(Guid id) => _collection.Find(Builders<T>.Filter.Eq("_id", id)).SingleOrDefault();
        
        public virtual void Insert(T document) => _collection.InsertOne(document);

        public virtual void Upsert(T document) => _collection.ReplaceOne(Builders<T>.Filter.Eq("_id", GetId(document)), document, new UpdateOptions{ IsUpsert = true });

        public virtual bool Update(T document) => _collection.ReplaceOne(Builders<T>.Filter.Eq("_id", GetId(document)), document).IsAcknowledged;
        
        public virtual bool Delete(Guid id) => _collection.DeleteOne(Builders<T>.Filter.Eq("_id", id)).IsAcknowledged;

        public virtual int Delete(Expression<Func<T, bool>> predicate) => (int)_collection.DeleteMany(predicate).DeletedCount;

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
