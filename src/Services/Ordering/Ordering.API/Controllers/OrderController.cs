using AutoMapper;
using Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Features.V1.Orders;
using Ordering.Applications.Features.V1.Orders.Commands.DeleteOrder;
using Ordering.Applications.Features.V1.Orders.Commands.UpdateOrder;
using Shared.Dtos.Order;
using Shared.SeedWord;
using Shared.Services.Email;
using System.ComponentModel.DataAnnotations;
using System.Net;
using OrderDto = Ordering.Application.Common.Models.OrderDto;

namespace Odering.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEmailService<MailRequest> _emailService;
        public OrderController(IMediator mediator, IOrderRepository repository, IMapper mapper, IEmailService<MailRequest> emailService)
        {
            _mediator = mediator ?? throw new ArgumentException(nameof(mediator));
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _emailService = emailService ?? throw new ArgumentException(nameof(emailService));
        }

        private static class RouteNames
        {
            public const string GetOrder = nameof(GetOrder);
            public const string CreateOrder = nameof(CreateOrder);
        }

        [HttpGet("{username}", Name = RouteNames.GetOrder)]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserName([Required][FromRoute] string username)
        {
            var query = new GetOrdersQuery(username);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost(Name = RouteNames.CreateOrder)]
        [ProducesResponseType(typeof(ApiResult<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResult<OrderDto>>> CreateOrder([FromBody] CreateOrderDto model)
        {
            try
            {
                var command = _mapper.Map<CreateOrderCommand>(model);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex is AppValidationException)
                {
                    var err = (AppValidationException)ex;
                    var firstError = err.Errors.First();
                    string message = firstError.Value.First();
                    return BadRequest(new ApiResult<OrderDto>(false, message));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult<OrderDto>(false, ex.Message));
            }
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(ApiResult<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResult<OrderDto>>> UpdateOrder([FromRoute] long id, [FromBody] UpdateOrderCommand command)
        {
            command.SetId(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(typeof(ApiResult<bool>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResult<bool>>> DeleteOrder([Required][FromRoute] long id)
        {
            var command = new DeleteOrderCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("/sendMail")]
        public async Task<IActionResult> SendMail([FromQuery] string email)
        {
            var mailRequest = new MailRequest
            {
                Body = "Test",
                Subject = "Test",
                To = email,
            };
            await _emailService.SendEmailAsync(mailRequest);
            return Ok();
        }
    }
}
