using FluentValidation;

namespace Ordering.Application.Features.V1.Orders;
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("{Username} is required vip.")
            .NotNull()
            .MaximumLength(50).WithMessage("{Username} must not exceed 50 characters.");
        RuleFor(x => x.EmailAddress)
            .NotEmpty().WithMessage("{EmailAddress} is required.")
            .NotNull()
            .EmailAddress().WithMessage("{EmailAddress} is not a valid email address.");
    }
}
