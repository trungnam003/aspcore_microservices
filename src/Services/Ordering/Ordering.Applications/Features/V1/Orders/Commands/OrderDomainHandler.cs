using Contracts.Services;
using MediatR;
using Ordering.Domain.OrderAggregate.Events;
using Serilog;
using Shared.Services.Email;

namespace Ordering.Applications.Features.V1.Orders.Commands
{
    public class OrderDomainHandler : INotificationHandler<OrderCreatedEvent>, INotificationHandler<OrderDeletedEvent>
    {
        private readonly ILogger _logger;
        private readonly IEmailService<MailRequest> _emailService;
        public OrderDomainHandler(ILogger logger, IEmailService<MailRequest> emailService) 
        { 
            _logger = logger;
            _emailService = emailService;
        }
        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.Information($"Ordering Domain Event: {notification.GetType().Name}");

            var email = new MailRequest
            {
                To = notification.EmailAddress,
                Body = $"Your order has been created. Order number is {notification.DocumentNo}" +
                $" and total price is {notification.TotalPrice}",
                Subject = $"Hello {notification.UserName}, your order was created",
            };
            try
            {
                _emailService.SendEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.Error($"Ordering Domain Event: {notification.GetType().Name} - Error: {ex.Message}");
                
            }
            return Task.CompletedTask;
        }

        public Task Handle(OrderDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.Information($"Ordering Domain Event: {notification.GetType().Name}");
            return Task.CompletedTask;
        }
    }
}
