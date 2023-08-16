using Contracts.Common.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Common.Interfaces
{
    public interface IOrderRepository : IRepositoryBase<Order, long>
    {
        Task<IEnumerable<Order>> GetOrdersByUsername(string username);
        Task CreateOrder(Order request);
        Task UpdateOrder(Order request);
        Task DeleteOrder(int id);
        Task<Order> CreateOrderMQ(Order order);
        Task<Order?> GetOrderByDocumentNo(string documentNo);
    }
}
