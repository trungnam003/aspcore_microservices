using Common.Logging;
using Infrastructure.Middlewares;
using Ocelot.Middleware;
using OcelotApi.Extensions;
using Serilog;

namespace OcelotApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            var environment = builder.Environment;
            var applicationName = environment.ApplicationName;
            var environmentName = environment.EnvironmentName ?? "Development";
            Log.Information($"Starting ({applicationName})-({environmentName})...");
            try
            {
                builder.Host.AddAppConfigurations();
                builder.Host.UseSerilog(SerilogLogger.Configure);
                // Add services to the container.

                builder.Services.AddConfigurationSettings(builder.Configuration);
                builder.Services.ConfigureOcelot(builder.Configuration);
                builder.Services.ConfigureCORS(builder.Configuration);
                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    //app.UseSwagger();
                    //app.UseSwaggerUI();
                }
                app.UseMiddleware<ErrorWrappingMiddleware>();
                app.UseCors("CorsPolicy");

                //app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseRouting();
                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGet("/", context =>
                    {
                        //await context.Response.WriteAsync($"{applicationName} is running...");
                        context.Response.Redirect("swagger/index.html");
                        return Task.CompletedTask;
                    });
                });

                //app.MapControllers();

                app.UseSwaggerForOcelotUI(opt =>
                {
                    opt.PathToSwaggerGenerator = "/swagger/docs";

                });

                await app.UseOcelot();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"{applicationName} terminated unexpectedly");
            }
            finally
            {
                Log.Fatal("Stopping Inventory.API");
                Log.CloseAndFlush();
            }
        }
    }
}