using Contracts.Common.Interfaces;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repositories
{
    public class CustomerRepository : RepositoryBase<Entities.Customer, int, CustomerContext>, ICustomerRepository
    {
        public CustomerRepository(CustomerContext context, IUnitOfWork<CustomerContext> unitOfWork) : base(context, unitOfWork)
        {
        }

        public async Task<Entities.Customer?> GetCustomerByUsernameAsync(string username)
        {
            return await FindByCondition(c => c.UserName == username).SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<Entities.Customer>> GetAllCustomer()
        {
            return await FindAll().ToListAsync();
        }

        public async Task CreateCustomerAsync(Entities.Customer customer)
        {
            await CreateAsync(customer);
        }

        public async Task UpdateCustomerAsync(Entities.Customer customer)
        {
            await UpdateAsync(customer);
        }
    }
}
