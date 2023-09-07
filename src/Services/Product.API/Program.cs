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

            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            var environment = builder.Environment;
            var applicationName = environment.ApplicationName;
            var environmentName = environment.EnvironmentName ?? "Development";
            Log.Information($"Starting ({applicationName})-({environmentName})...");

            try
            {
                builder.Host.UseSerilog(SerilogLogger.Configure);
                builder.Host.AddAppConfigurations();
                
                builder.Services.AddConfigurationSettings(builder.Configuration);
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

                //app.Run();
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