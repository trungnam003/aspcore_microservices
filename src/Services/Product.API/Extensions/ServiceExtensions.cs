using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product.API.Persistence;
using Product.API.Repositories;
using Product.API.Repositories.Interfaces;
using System.Reflection;

namespace Product.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.ConfigureProductDbContext(configuration);

            services.AddInfrastructureServices();

            services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
            //services.AddAutoMapper(Assembly.GetExecutingAssembly()); Tự động nhận các profile trong assembly

            return services;
        }

        private static IServiceCollection ConfigureProductDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var builder = new MySqlConnectionStringBuilder(connectionString);


            services.AddDbContext<ProductContext>(options =>
            {
                options.UseMySql(builder.ConnectionString,
                    ServerVersion.AutoDetect(builder.ConnectionString),
                    e =>
                    {
                        e.MigrationsAssembly(typeof(ProductContext).Assembly.FullName);
                        e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                    });
            });
            return services;
        }

        private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // gán kiểu cho các interface có Type Generic 
            services.AddScoped(typeof(IRepositoryBase<,,>), typeof(RepositoryBase<,,>))
                .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            // gán kiểu cho các interface không có Type Generic
            services.AddScoped<IProductRepository, ProductRepository>();


            return services;
        }
    }


}
