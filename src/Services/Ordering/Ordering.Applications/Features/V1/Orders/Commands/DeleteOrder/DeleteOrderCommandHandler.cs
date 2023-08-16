using AutoMapper;
using MediatR;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Serilog;

namespace Ordering.Applications.Features.V1.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;
        private readonly ILogger _logger;
        private const string MethodName = nameof(DeleteOrderCommandHandler);

        public DeleteOrderCommandHandler(IMapper mapper, IOrderRepository repository, ILogger logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _repository.FindByCondition(x => x.Id == request.Id).FirstOrDefault();
            if (order == null)
            {
                _logger.Error("Order Not Found");
                throw new NotFoundException("Order Not Found");
            }
            await _repository.DeleteAsync(order);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
