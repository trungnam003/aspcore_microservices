using AutoMapper;
using EventBus.Messages.IntegrationEvents.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.V1.Orders;
using ILogger = Serilog.ILogger;

namespace Ordering.API.Applications.IntegrationEvents.EventsHandler
{
    public class BasketCheckoutEventHandler : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public BasketCheckoutEventHandler(IMediator mediator, IMapper mapper, ILogger logger)
        {
            _mediator = mediator ?? throw new ArgumentException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _logger = logger ?? throw new ArgumentException(nameof(logger)); ;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = _mapper.Map<CreateOrderCommand>(context.Message);
            var result = await _mediator.Send(command);
            _logger.Information($"BasketCheckoutEvent consumed successfully. Created Order Id : {result.Data.Id}");

        }
    }
}
