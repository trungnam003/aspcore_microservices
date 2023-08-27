using Inventory_gRPC.Protos;

namespace Basket.API.GrpcServices
{
    public class StockItemGrpcService
    {
        private readonly StockProtoService.StockProtoServiceClient _stockProtoServiceClient;

        public StockItemGrpcService(StockProtoService.StockProtoServiceClient stockProtoServiceClient)
        {
            _stockProtoServiceClient = stockProtoServiceClient ?? throw new ArgumentNullException(nameof(stockProtoServiceClient));
        }

        public async Task<StockModel> GetStock(string itemNo)
        {
            try
            {
                var stockItemRequest = new GetStockRequest
                {
                    ItemNo = itemNo
                };

                return await _stockProtoServiceClient.GetStockAsync(stockItemRequest);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
