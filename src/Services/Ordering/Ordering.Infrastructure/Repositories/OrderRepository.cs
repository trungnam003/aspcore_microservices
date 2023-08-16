using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order, long, OrderContext>, IOrderRepository
    {
        public OrderRepository(OrderContext context, IUnitOfWork<OrderContext> unitOfWork) : base(context, unitOfWork)
        {
        }

        public async Task CreateOrder(Order request)
        {
            await CreateAsync(request);
        }

        public Task<Order> CreateOrderMQ(Order order)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOrder(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Order?> GetOrderByDocumentNo(string documentNo)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUsername(string username)
        {
            return await FindByCondition(x => x.UserName.Equals(username))
                .ToListAsync();

        }

        public async Task UpdateOrder(Order request)
        {
            await UpdateAsync(request);
        }
    }
}
