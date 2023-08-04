using Contracts.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;

namespace Product.API.Persistence
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {

        }

        public DbSet<CatalogProduct> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CatalogProduct>().HasIndex(x => x.No).IsUnique();
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
