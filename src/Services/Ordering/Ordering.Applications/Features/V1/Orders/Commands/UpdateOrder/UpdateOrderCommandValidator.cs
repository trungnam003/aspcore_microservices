using FluentValidation;

namespace Ordering.Applications.Features.V1.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("{Id} is required.")
                .ChildRules(x => x.RuleFor(x => x).GreaterThan(0)).WithMessage("{Id} must be greater than 0.");
        }
    }
}
