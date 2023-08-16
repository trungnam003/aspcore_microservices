using FluentValidation;

namespace Ordering.Application.Features.V1.Orders
{
    public class CreateOrUpdateCommandValidator : AbstractValidator<CreateOrUpdateCommand>
    {
        public CreateOrUpdateCommandValidator()
        {
            RuleFor(x => x.FirstName)
               .NotEmpty().WithMessage("{FirstName} is required.")
               .NotNull()
               .MaximumLength(50).WithMessage("{FirstName} must not exceed 50 characters.");

            RuleFor(x => x.LastName)
               .NotEmpty().WithMessage("{LastName} is required.")
               .NotNull()
               .MaximumLength(50).WithMessage("{LastName} must not exceed 50 characters.");

            RuleFor(x => x.EmailAddress)
               .EmailAddress().WithMessage("{EmailAddress} is invalid format.")
               .NotEmpty().WithMessage("{EmailAddress} is required.");

            RuleFor(x => x.TotalPrice)
             .NotEmpty().WithMessage("{TotalPrice} is required.")
             .GreaterThan(0).WithMessage("{TotalPrice} should  be greater than zero.");
        }
    }
}
