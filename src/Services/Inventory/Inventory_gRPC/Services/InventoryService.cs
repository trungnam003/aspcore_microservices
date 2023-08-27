
using Grpc.Core;
using Inventory_gRPC.Protos;
using Inventory_gRPC.Repositories.Interfaces;
using ILogger = Serilog.ILogger;

namespace Inventory_gRPC.Services
{
    public class InventoryService : StockProtoService.StockProtoServiceBase
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ILogger _logger;

        public InventoryService(IInventoryRepository inventoryRepository, ILogger logger)
        {
            _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        override public async Task<StockModel> GetStock(GetStockRequest request, ServerCallContext context)
        {
            _logger.Information($"Begin Get Stock of ItemNo {request.ItemNo}");
            var stockQuantity = _inventoryRepository.GetStockQuantity(request.ItemNo);
            var result = new StockModel
            {
                Quantity = stockQuantity
            };
            _logger.Information($"End Get Stock of ItemNo {request.ItemNo} - Quantity {result.Quantity}");
            return result;

        }
    }
}
