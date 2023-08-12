using Common.Logging;
using Serilog;
namespace Inventory.API
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
                {
                    // Add services to the container.
                    builder.Services.AddControllers();
                    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                    builder.Services.AddEndpointsApiExplorer();
                    builder.Services.AddSwaggerGen();
                }

                var app = builder.Build();
                {
                    // Configure the HTTP request pipeline.
                    if (app.Environment.IsDevelopment())
                    {
                        app.UseSwagger();
                        app.UseSwaggerUI();
                    }

                    app.UseHttpsRedirection();

                    app.UseAuthorization();


                    app.MapControllers();
                }

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Inventory.API terminated unexpectedly");
            }
            finally
            {
                Log.Fatal("Stopping Inventory.API");
                Log.CloseAndFlush();
            }
        }
    }
}