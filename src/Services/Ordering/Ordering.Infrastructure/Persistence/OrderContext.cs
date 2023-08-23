using Contracts.Common.Events;
using Contracts.Common.Interfaces;
using Contracts.Domains.Interfaces;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Serilog;
using System.Reflection;

#nullable disable

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContext : DbContext
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public OrderContext(DbContextOptions<OrderContext> options, IMediator mediator, ILogger logger) : base(options)
        {
            //dotnet ef migrations add "Init_OrderDb"--project Ordering.Infrastructure--startup - project Odering.API--output - dir Ordering.Infrastructure\Persistence / Migrations
            //dotnet ef migrations remove --project Ordering.Infrastructure--startup - project Odering.API
            _mediator = mediator;
            _logger = logger;
        }

        public DbSet<Order> Orders { get; set; }
        private List<BaseEvent> _baseEvents;

        private void SetBaseEventsBeforeSaveChanges()
        {
            var domainEntities = ChangeTracker
               .Entries<IEventEntity>()
               .Select(e => e.Entity)
               .Where(e => e.GetDomainEvents().Any())
               .ToList();

            _baseEvents = domainEntities
                .SelectMany(e => e.GetDomainEvents())
                .ToList();

            domainEntities.ForEach(entity => entity.ClearDomainEvents());
        }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SetBaseEventsBeforeSaveChanges();
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
            var result = await base.SaveChangesAsync(cancellationToken);
            await _mediator.DispatchDomainEventAsync(_baseEvents, _logger);

            return result;
        }

    }
}
