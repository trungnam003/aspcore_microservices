using Basket.API.Extensions;
using Common.Logging;
using Serilog;
namespace Basket.API
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

                {
                    // Add services to the container.
                    builder.Services.AddControllers();
                    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                    builder.Services.AddEndpointsApiExplorer();
                    builder.Services.AddSwaggerGen();
                    builder.Services.Configure<RouteOptions>(options
                        => options.LowercaseUrls = true);

                    builder.Services.ConfigureServices();
                    builder.Services.ConfigureRedis(builder.Configuration);
                }

                var app = builder.Build();
                {
                    Log.Information("Starting Basket.API");
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

                app.Run();
            }
            catch (Exception ex)
            {
                string type = ex.GetType().Name;
                if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

                Log.Fatal(ex, "Basket.API terminated unexpectedly");
            }
            finally
            {
                Log.Fatal("Stopping Basket.API");
                Log.CloseAndFlush();
            }
        }
    }
}