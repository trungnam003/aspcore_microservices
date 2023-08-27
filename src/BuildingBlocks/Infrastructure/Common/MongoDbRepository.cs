using Contracts.Domains;
using MongoDB.Driver;
using System.Linq.Expressions;
using Contracts.Common.Interfaces;
using Shared.Configurations;
using Infrastructure.Extensions;
#nullable disable
namespace Infrastructure.Common
{
    public class MongoDbRepository<T> : IMongoDbRepositoryBase<T> where T : MongoEntity
    {
        private IMongoDatabase Database { get; }
        protected virtual IMongoCollection<T> Collection =>
           Database.GetCollection<T>(GetCollectionName());
        public MongoDbRepository(IMongoClient database, MongoDbDatabaseSettings databaseSettings)
        {
            Database = database.GetDatabase(databaseSettings.DatabaseName);
        }

        public Task CreateAsync(T entity)
        {
            return Collection.InsertOneAsync(entity);
        }

        public Task DeleteAsync(string id)
        {
            return Collection.DeleteOneAsync(x => x.Id == id);
        }

        public IMongoCollection<T> FindAll(ReadPreference readPreference = null)
        {
            return Database.WithReadPreference(readPreference ?? ReadPreference.Primary)
                .GetCollection<T>(GetCollectionName());
        }

        public Task UpdateAsync(T entity)
        {
            Expression<Func<T, string>> func = x => x.Id;
            var value = (string)entity.GetType()
                .GetProperty(func.Body.ToString().Split(".")[1])
                ?.GetValue(entity, null);

            var filter = Builders<T>.Filter.Eq(func, value);

            return Collection.ReplaceOneAsync(filter, entity);
        }

        private static string GetCollectionName()
        {
            var collectionName = (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                .FirstOrDefault() as BsonCollectionAttribute)?.CollectionName;
            return collectionName;
        }


    }
}
