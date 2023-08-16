using Contracts.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using System.Reflection;

#nullable disable

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
            //dotnet ef migrations add "Init_OrderDb"--project Ordering.Infrastructure--startup - project Odering.API--output - dir Ordering.Infrastructure\Persistence / Migrations
            //dotnet ef migrations remove --project Ordering.Infrastructure--startup - project Odering.API
        }

        public DbSet<Order> Orders { get; set; }
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var modifiredEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified ||
                            e.State == EntityState.Added ||
                            e.State == EntityState.Deleted);

            foreach (var entry in modifiredEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity is IDateTracking addedEntity)
                        {
                            addedEntity.CreatedDate = DateTime.UtcNow;
                            entry.State = EntityState.Added;
                        }
                        break;

                    case EntityState.Modified:
                        Entry(entry.Entity).Property("Id").IsModified = false;
                        if (entry.Entity is IDateTracking modifiedEntity)
                        {
                            modifiedEntity.LastModifiedDate = DateTime.UtcNow;
                            entry.State = EntityState.Modified;
                        }
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
