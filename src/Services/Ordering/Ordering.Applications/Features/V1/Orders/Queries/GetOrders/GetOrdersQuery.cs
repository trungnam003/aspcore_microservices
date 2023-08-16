using MediatR;
using Ordering.Application.Common.Models;
using Shared.SeedWord;

namespace Ordering.Application.Features.V1.Orders
{
    public class GetOrdersQuery : IRequest<ApiResult<List<OrderDto>>>
    {
        public string Username { get; set; }

        public GetOrdersQuery(string username)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
        }
    }
}
