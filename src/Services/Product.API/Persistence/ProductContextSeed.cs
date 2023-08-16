using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using ILogger = Serilog.ILogger;

namespace Product.API.Persistence
{
    public class ProductContextSeed
    {
        public static async Task SeedProductAsync(ProductContext productContext, ILogger logger)
        {
            if (!productContext.Products.Any() && productContext.Database.IsMySql())
            {
                productContext.Products.AddRange(GetCatalogProducts());
                await productContext.SaveChangesAsync();
                logger.Information("Seed database associated with context {DbContextName}", typeof(ProductContext).Name);
            }
        }

        private static IEnumerable<CatalogProduct> GetCatalogProducts()
        {
            return new List<CatalogProduct>()
            {
                new()
                {
                    No = "Lotus",
                    Name = "Esprit",
                    Sumary = "This is Lotus Esprit",
                    Description = "This is Lotus Esprit",
                    Price = (decimal)12873.33,
                },
                new()
                {
                    No = "Car",
                    Name = "Lamborghini Aventador",
                    Sumary = "This is Lamborghini Aventador",
                    Description = "This is Lamborghini Aventador",
                    Price = (decimal)37123.93,
                }
            };
        }
    }
}
