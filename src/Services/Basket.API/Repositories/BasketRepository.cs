using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using ILogger = Serilog.ILogger;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;
        private readonly ISerializeService _serializeService;
        private readonly ILogger _logger;

        public BasketRepository(IDistributedCache redisCache, ISerializeService serializeService, ILogger logger)
        {
            _serializeService = serializeService;
            _redisCache = redisCache;
            _logger = logger;
        }

        public async Task<bool> DeleteBasketByUserName(string userName)
        {
            try
            {
                await _redisCache.RemoveAsync(userName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error deleting basket for user {userName}");
                _logger.Error(ex.Message);
                throw;
            }
        }

        public async Task<Cart?> GetBasketByUserName(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            return string.IsNullOrEmpty(basket) ? null :
                _serializeService.Deserialize<Cart>(basket);
        }

        public async Task<Cart?> UpdateBasket(Cart basket, DistributedCacheEntryOptions? options = null)
        {
            if (options != null)
            {

                await _redisCache.SetStringAsync(basket.UserName,
                                       _serializeService.Serialize(basket), options);
            }
            else
            {
                await _redisCache.SetStringAsync(basket.UserName,
                                       _serializeService.Serialize(basket));
            }

            return await GetBasketByUserName(basket.UserName);
        }
    }
}
