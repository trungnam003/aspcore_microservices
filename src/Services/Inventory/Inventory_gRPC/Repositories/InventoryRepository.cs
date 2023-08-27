using Infrastructure.Common;
using Inventory_gRPC.Entities;
using Inventory_gRPC.Repositories.Interfaces;
using MongoDB.Driver;
using Shared.Configurations;

namespace Inventory_gRPC.Repositories
{
    public class InventoryRepository : MongoDbRepository<InventoryEntry>, IInventoryRepository
    {
        public InventoryRepository(IMongoClient database, MongoDbDatabaseSettings databaseSettings) : base(database, databaseSettings)
        {
        }

        public int GetStockQuantity(string itemNo)
        {   
            return Collection.AsQueryable()
                .Where(x => x.ItemNo == itemNo)
                .Sum(x => x.Quantity);
        }
    }
}
