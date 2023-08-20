using AutoMapper;
using EventBus.Messages.IntegrationEvents.Events;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Domain.Entities;
using Shared.Dtos.Order;
using Shared.SeedWord;
using OrderDto = Ordering.Application.Common.Models.OrderDto;

namespace Ordering.Application.Features.V1.Orders
{
    public class CreateOrderCommand : CreateOrUpdateCommand, IRequest<ApiResult<OrderDto>>, IMapFrom<Order>
    {
        public string Username { get; set; }
        public new void Mapping(Profile profile)
        {
            profile.CreateMap<CreateOrderDto, CreateOrderCommand>();
            profile.CreateMap<CreateOrderCommand, Order>();
            profile.CreateMap<CreateOrderCommand, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
