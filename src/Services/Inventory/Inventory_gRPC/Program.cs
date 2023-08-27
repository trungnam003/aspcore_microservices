
using Common.Logging;
using Inventory_gRPC.Extensions;
using Inventory_gRPC.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

namespace Inventory_gRPC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog(SerilogLogger.Configure);
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            
            Log.Information($"Starting rGPC...");

            try
            {
                // Additional configuration is required to successfully run gRPC on macOS.
                // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
                builder.Services.AddGrpc();

                // Add services to the container.
                builder.Services.AddInfrastrutureServices();
                builder.Services.AddConfigurationSettings(builder.Configuration);
                builder.Services.ConfigureMongoDb();

               
                var app = builder.Build();

                // Configure the HTTP request pipeline.
                app.MapGrpcService<InventoryService>();
                app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Inventory.gRPC terminated unexpectedly");
            }
            finally
            {
                Log.Fatal("Stopping Inventory.gRPC");
                Log.CloseAndFlush();
            }
        }
    }
}