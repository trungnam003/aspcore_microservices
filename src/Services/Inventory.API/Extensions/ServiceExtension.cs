using Infrastructure.Extensions;
using Inventory.API.Services;
using Inventory.API.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
#nullable disable
namespace Inventory.API.Extensions
{
    public static class ServiceExtension
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddAutoMapper((cfg) => cfg.AddProfile(new MappingProfile()));

            services.AddScoped<IInventoryService, InventoryService>();
        }

        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDbSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
            services.AddSingleton(mongoDbSettings);
            return services;
        }

        private static string getMongoConnectionString(IServiceCollection services)
        {
            var settings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
            if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
            {
               throw new ArgumentNullException(nameof(settings.ConnectionString));
            }
            var databaseName = settings.DatabaseName;
            var mongoDbConnectionString = $"{settings.ConnectionString}/{databaseName}?authSource=admin";
            return mongoDbConnectionString;
        }

        public static void ConfigureMongoDb(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                string mongoDbConnectionString = getMongoConnectionString(services);

                return new MongoClient(mongoDbConnectionString);
            }).AddScoped(x => x.GetService<IMongoClient>()?.StartSession());
            
        }
    }
}
