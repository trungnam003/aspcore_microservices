using Common.Logging;
using Contracts.Common.Interfaces;
using Customer.API.Controllers;
using Customer.API.Persistence;
using Customer.API.Repositories;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services;
using Customer.API.Services.Interfaces;
using Customer.API.Utils.Validations;
using FluentValidation;
using Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.DTOs.Customer;

namespace Customer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog(SerilogLogger.Configure);

            try
            {

                // Add services to the container.
                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                builder.Services.AddDbContext<CustomerContext>(options => options.UseNpgsql(connectionString));

                builder.Services.AddAutoMapper(cfgs => cfgs.AddProfile(new MappingProfile()));

                builder.Services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>));
                builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
                builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

                builder.Services.AddScoped<ICustomerService, CustomerService>();

                // Fluent Validation
                builder.Services.AddScoped<IValidator<UpdateCustomerDto>, UpdateCustomerValidation>();


                var app = builder.Build();
                {
                    Log.Information("Starting Customer.API");
                    // Configure the HTTP request pipeline.
                    if (app.Environment.IsDevelopment())
                    {
                        app.UseSwagger();
                        app.UseSwaggerUI();
                    }

                    app.UseAuthorization();

                    // Minimal API
                    app.MapCustomerController();

                    //app.MapControllers();
                }

                app.SeedData();
                app.Run();
            }
            catch (Exception ex)
            {
                string type = ex.GetType().Name;
                if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

                Log.Fatal(ex, "Customer.API terminated unexpectedly");
            }
            finally
            {
                Log.Fatal("Stopping Customer.API");
                Log.CloseAndFlush();
            }
        }
    }
}