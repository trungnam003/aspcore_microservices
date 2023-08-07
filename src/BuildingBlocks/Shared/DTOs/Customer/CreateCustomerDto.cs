using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Shared.DTOs.Customer
{
    public class CreateCustomerDto
    {
        [Required]
        [MinLength(4)]
        [MaxLength(64)]
        public string UserName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(150)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        
        public bool IsValid()
        {
            return Validator.TryValidateObject(this, new ValidationContext(this), null, true);
        }
        public ValidationResult GetValidationResult()
        {
            if(!IsValid())
            {
                var validationResults = new List<ValidationResult>();
                Validator.TryValidateObject(this, new ValidationContext(this), validationResults, true);
                return validationResults.FirstOrDefault();
            }else
            {
                return ValidationResult.Success;
            }
        }
    }
}
