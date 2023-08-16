using Infrastructure.Configurations;

namespace Ordering.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection(nameof(EmailSMTPSettings));
            services.Configure<EmailSMTPSettings>(emailSettings);

            return services;
        }
    }
}
