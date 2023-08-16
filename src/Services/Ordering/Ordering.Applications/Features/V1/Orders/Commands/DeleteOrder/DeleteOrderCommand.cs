using MediatR;

namespace Ordering.Applications.Features.V1.Orders.Commands.DeleteOrder
{

    public class DeleteOrderCommand : IRequest<bool>
    {
        public long Id { get; set; }

        public DeleteOrderCommand(long id)
        {
            Id = id;
        }
    }
}
