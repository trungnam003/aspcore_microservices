using AutoMapper;
using Infrastructure.Mapping;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Application.Features.V1.Orders;
using Ordering.Domain.Entities;
using Shared.SeedWord;
using OrderDto = Ordering.Application.Common.Models.OrderDto;

namespace Ordering.Applications.Features.V1.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : CreateOrUpdateCommand, IRequest<ApiResult<OrderDto>>, IMapFrom<Order>
    {
        public long Id { get; private set; }

        public void SetId(long id)
        {
            Id = id;
        }

        public new void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateOrderCommand, Order>()
                .ForMember(d => d.Status, opt => opt.Ignore())
                .IgnoreAllNonExisting().ReverseMap();
        }
    }
}
