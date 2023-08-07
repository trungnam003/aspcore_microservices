using FluentValidation;
using Shared.DTOs.Customer;

namespace Customer.API.Utils.Validations
{
    public class UpdateCustomerValidation : AbstractValidator<UpdateCustomerDto>
    {
        public UpdateCustomerValidation()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required");
            RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress().WithMessage("Email is required");
        }
    }
}
