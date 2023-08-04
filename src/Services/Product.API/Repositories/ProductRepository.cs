using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Persistence;
using Product.API.Repositories.Interfaces;

namespace Product.API.Repositories
{
    public class ProductRepository : RepositoryBaseAsync<CatalogProduct, long, ProductContext>, IProductRepository
    {
        public ProductRepository(ProductContext dbContext, IUnitOfWork<ProductContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public async Task<CatalogProduct?> GetProductAsync(long id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<CatalogProduct?> GetProductByNoAsync(string productNo)
        {
            return await FindByCondition(x => x.No.Equals(productNo)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CatalogProduct>> GetProductsAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task CreateProductAsync(CatalogProduct product)
        {
            await CreateAsync(product);
        }

        public async Task DeleteProductAsync(long id)
        {
            var product = await GetProductAsync(id);
            if(product != null)
            {
                await DeleteAsync(product);
            }
        }

        public async Task UpdateProductAsync(CatalogProduct product)
        {
            await UpdateAsync(product);
        }
    }
}
