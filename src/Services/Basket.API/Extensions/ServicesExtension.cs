using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using Infrastructure.Common;

namespace Basket.API.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddTransient<ISerializeService, SerializeService>();
            //services.AddTransient<IDistributedCacheService, DistributedCacheService>(); 
            return services;
        }

        public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisSettings = configuration.GetSection("CacheSettings:ConnectionString").Value;
            if (string.IsNullOrEmpty(redisSettings))
            {
                throw new ArgumentNullException(nameof(redisSettings));
            }

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisSettings;
            });


        }
    }
}
