using Common.Logging;
using Product.API.Extensions;
using Product.API.Persistence;
using Serilog;

namespace Product.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            try
            {
                builder.Host.UseSerilog(SerilogLogger.Configure);
                builder.Host.AddAppConfigurations();

                builder.Services.ConfigureServices(builder.Configuration);

                var app = builder.Build();

                app.UseApplication();

                app.MigrateDatabase<ProductContext>(
                        (context, _) =>
                        {
                            ProductContextSeed.SeedProductAsync(context, Log.Logger).Wait();
                        }
                    )
                    .Run();

                app.Run();
            }
            catch (Exception ex)
            {
                string type = ex.GetType().Name;

                if (type == "StopTheHostException")
                    throw;

                Log.Fatal($"Product.API terminated unexpectedly {ex}");
            }

            finally
            {
                Log.Fatal("Stopping Product.API");
                Log.CloseAndFlush();
            }
        }
    }
}