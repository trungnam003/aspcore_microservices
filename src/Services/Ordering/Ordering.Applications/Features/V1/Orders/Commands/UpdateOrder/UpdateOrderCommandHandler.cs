using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWord;

namespace Ordering.Applications.Features.V1.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResult<OrderDto>>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;
        private readonly ILogger _logger;
        private const string MethodName = "UpdateOrderHandler";

        public UpdateOrderCommandHandler(IMapper mapper, IOrderRepository repository, ILogger logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiResult<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName}");

            var order = await _repository.FindByCondition(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (order != null)
            {
                // khi cần cập nhật dữ liệu cần có đối tượng cần cập nhật nhỏ -> lớn
                order = _mapper.Map<UpdateOrderCommand, Order>(request, order);
                // khi không cần cập nhật dữ liệu và tạo đối tượng mới không cần có đối tượng cần cập nhật lớn -> nhỏ
                //order = _mapper.Map<Order>(request);
                await _repository.UpdateOrder(order);
                await _repository.SaveChangesAsync();

                _logger.Information($"END: {MethodName}");

                return new ApiSuccessResult
                    <OrderDto>(_mapper.Map<OrderDto>(order));
            }
            else
            {
                throw new Exception("Error update");
            }
        }
    }
}
