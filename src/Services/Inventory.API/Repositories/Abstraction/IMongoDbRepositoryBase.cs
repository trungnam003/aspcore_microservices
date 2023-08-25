using Inventory.API.Entities.Abstraction;
using MongoDB.Driver;

namespace Inventory.API.Repositories.Abstraction
{
    public interface IMongoDbRepositoryBase<T> where T: MongoEntity
    {
        IMongoCollection<T> FindAll(ReadPreference? readPreference = null);

        Task CreateAsync(T entity);
        Task DeleteAsync(string id);
        Task UpdateAsync(T entity);
    }
}
