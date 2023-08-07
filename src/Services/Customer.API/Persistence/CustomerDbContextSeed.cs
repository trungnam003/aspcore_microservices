using Microsoft.EntityFrameworkCore;

namespace Customer.API.Persistence
{
    public static class CustomerDbContextSeed
    {
        public static IHost SeedData(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<CustomerContext>();
                context.Database.MigrateAsync().GetAwaiter().GetResult();
                SeedCustomers(context).GetAwaiter().GetResult();
            }
            return host;
        }
        private static async Task SeedCustomers(this CustomerContext context)
        {
            if (context.Customers.Any()) return;

            var customers = new List<Entities.Customer>
            {
                new Entities.Customer
                {
                    FirstName = "John",
                    LastName = "Doe",
                    EmailAddress = "test@email.com",
                    UserName = "johndoe",
                },
                new Entities.Customer
                {
                    FirstName = "Jack",
                    LastName = "Ma",
                    EmailAddress = "jackma@email.com",
                    UserName = "jackma",
                }
            };
            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }
    }
    
}
