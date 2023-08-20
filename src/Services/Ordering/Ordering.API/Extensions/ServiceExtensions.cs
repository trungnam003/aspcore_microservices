using EventBus.Messages.IntegrationEvents.Events;
using Infrastructure.Configurations;
using Infrastructure.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.API.Applications.IntegrationEvents.EventsHandler;
using Shared.Configurations;

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
        public static void ConfigureMassTransit(this IServiceCollection services)
        {
            var settings = services.GetOptions<EventBusSettings>(nameof(EventBusSettings));
            if (string.IsNullOrEmpty(settings.HostAddress))
            {
                throw new ArgumentNullException(nameof(settings));
            }
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

            services.AddMassTransit(config =>
            {
                config.AddConsumersFromNamespaceContaining<BasketCheckoutEventHandler>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    
                    cfg.Host(settings.HostAddress);
                    //cfg.ReceiveEndpoint("basket-checkout-queue", c =>
                    //{
                    //    c.ConfigureConsumer<BasketCheckoutEventHandler>(ctx);
                    //});

                    cfg.ConfigureEndpoints(ctx);
                });
            });
        }  
    }
}
