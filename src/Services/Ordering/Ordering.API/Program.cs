using Common.Logging;
using Contracts.Common.Interfaces;
using Contracts.Messages;
using Infrastructure.Common;
using Infrastructure.Messages;
using Ordering.API.Extensions;
using Ordering.Applications;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Serilog;
using System.Diagnostics;

namespace Odering.API
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
                {
                    builder.Services.AddScoped<ISerializeService, SerializeService>();
                    builder.Services.AddSingleton<Stopwatch>(new Stopwatch());
                    builder.Services.AddScoped<IMessageProducer, RabbitMQProducer>();
                    // Add services to the container.
                    builder.Services.AddControllers();
                    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                    builder.Services.AddEndpointsApiExplorer();
                    builder.Services.AddSwaggerGen();

                    builder.Services.AddConfigurationSettings(builder.Configuration);
                    builder.Services.AddInfrastructureServices(builder.Configuration);
                    builder.Services.AddApplicationServices();
                }

                var app = builder.Build();
                {
                    // Configure the HTTP request pipeline.
                    if (app.Environment.IsDevelopment())
                    {
                        app.UseSwagger();
                        app.UseSwaggerUI();
                    }

                    //app.UseHttpsRedirection();

                    app.UseAuthorization();


                    app.MapControllers();
                }

                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<OrderContextSeed>();
                        context.InitialiseAsync().Wait();
                        context.SeedAsync().Wait();
                    }
                    catch (Exception ex)
                    {
                        Log.Fatal(ex, "An error occurred while migrating or seeding the database.");
                    }
                }
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
                Log.Fatal("Stopping Odering.API");
                Log.CloseAndFlush();
            }
        }
    }
}