
#nullable disable
using Contracts.Identity;
using Infrastructure.Extensions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Cache;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;
using Shared.Configurations;
using System.Text;

namespace OcelotApi.Extensions
{
    public static class ServiceExtension
    {

        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            //var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
            //services.AddSingleton(jwtSettings);
            return services;
        }

        public static IServiceCollection ConfigureOcelot(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOcelot(configuration)
                .AddPolly()
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                });

            //services.AddSingleton<IOcelotCache<>>
            //services.AddTransient<ITokenService, TokenService>();
            //services.AddJwtAuthentication();

            services.AddSwaggerForOcelot(configuration, x =>
            {
                x.GenerateDocsForGatewayItSelf = false;
            });

            return services;
        }
        internal static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            var settings = services.GetOptions<JwtSettings>(nameof(JwtSettings));
            if (settings == null || string.IsNullOrEmpty(settings.Key))
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
        public static void ConfigureCORS(this IServiceCollection services, IConfiguration configuration)
        {
            var origins = configuration.GetSection("AllowOrigins").Value.Split(',');
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                      builder =>
                      {
                          builder.WithOrigins(origins)
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      });
            });
        }
    }
}
