using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Serilog;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        private readonly OrderContext _context;
        private readonly ILogger _logger;

        public OrderContextSeed(OrderContext context, ILogger logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while seeding the database with orders.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                if (!_context.Orders.Any())
                {
                    await TrySeedAsync();
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while seeding the database with orders.");
                throw;
            }
        }

        private async Task TrySeedAsync()
        {
            if (!_context.Orders.Any())
            {
                await _context.Orders.AddRangeAsync(new List<Order>
                {
                    new Order
                    {
                        UserName = "cus1",
                        FirstName = "John",
                        LastName = "Doe",
                        EmailAddress = "join@email.com",
                        ShippingAddress = "123 London",
                        TotalPrice = 350,
                        InvoiceAddress = "321 London",
                    },
                    new Order
                    {
                        UserName = "cus2",
                        FirstName = "Even",
                        LastName = "Chip",
                        EmailAddress = "chip123@email.com",
                        ShippingAddress = "123 New york",
                        TotalPrice = 450,
                        InvoiceAddress = "321 London",
                    }
                });
            }
        }
    }
}
