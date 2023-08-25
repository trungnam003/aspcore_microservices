using Inventory.API.Persistence;
using MongoDB.Driver;

namespace Inventory.API.Extensions
{
    public static class HostExtension
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var settings = services.GetService<DatabaseSettings>();
            if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
            {
                throw new ArgumentNullException(nameof(settings.ConnectionString));
            }
            var mongoClient = services.GetRequiredService<IMongoClient>();
            if(mongoClient == null)
            {
                throw new ArgumentNullException(nameof(mongoClient));
            }
            new InventoryDbSeed().SeedDataAsync(mongoClient, settings).Wait();
            return host;
        }
    }
}
