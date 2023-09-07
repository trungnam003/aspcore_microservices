using Contracts.Common.Interfaces;
using Contracts.Identity;
using Infrastructure.Common;
using Infrastructure.Extensions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product.API.Persistence;
using Product.API.Repositories;
using Product.API.Repositories.Interfaces;
using Shared.Configurations;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;

namespace Product.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
            services.AddSingleton(jwtSettings);

            //services.AddSwaggerGen((options) =>
            //{
                
            //    options.OperationFilter<SecurityRequirementsOperationFilter>(true, "Bearer");
            //    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        Description = "Standard Authorization header using the Bearer scheme (JWT). Example: \"bearer {token}\"",
            //        Name = "Authorization",
            //        In = ParameterLocation.Header,
            //        Type = SecuritySchemeType.Http,
            //        BearerFormat = "JWT",
            //        Scheme = "Bearer"
            //    });

            //});
            return services;
        }

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

            //services.AddJwtAuthentication();

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

        internal static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            var settings = services.GetOptions<JwtSettings>(nameof(JwtSettings));
            if(settings == null || string.IsNullOrEmpty(settings.Key))
            {
                throw new ArgumentNullException(nameof(JwtSettings));
            }
            var singingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = singingKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = false
            };

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false; // chỉ sử dụng https
                x.SaveToken = true; // lưu token vào HttpContext
                x.TokenValidationParameters = tokenValidationParameters;
            });

            return services;
        }

    }


}
