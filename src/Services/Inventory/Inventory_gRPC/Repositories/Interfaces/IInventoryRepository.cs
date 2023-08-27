using Contracts.Common.Interfaces;
using Inventory_gRPC.Entities;

namespace Inventory_gRPC.Repositories.Interfaces
{
    public interface IInventoryRepository : IMongoDbRepositoryBase<InventoryEntry>
    {
        int GetStockQuantity(string itemNo);
    }
}
