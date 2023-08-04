using Microsoft.EntityFrameworkCore;

namespace Product.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
            where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                TContext? context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");
                    if (context == null)
                    {
                        throw new Exception($"Context {typeof(TContext).Name} is null");
                    }
                    ExecuteMigrations<TContext>(context);

                    logger.LogInformation($"Seeding database associated with context {typeof(TContext).Name}");
                    InvokeSeeder(seeder, context, services);

                }
                catch (Exception ex)
                {
                    logger.LogError($"{ex} An Error occured while migration the MySQL Product Database");
                }
            }
            return host;
        }

        private static void ExecuteMigrations<TContext>(TContext context)
            where TContext : DbContext
        {
            context.Database.Migrate();
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder,
            TContext context, IServiceProvider services)
            where TContext : DbContext
        {
            seeder(context, services);
        }
    }
}
