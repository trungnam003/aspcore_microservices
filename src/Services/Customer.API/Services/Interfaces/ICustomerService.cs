using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Customer;

namespace Customer.API.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IResult> GetCustomerByUsernameAsync(string username);

        Task<IResult> GetAllCustomer();

        Task<IResult> CreateCustomerAsync(CreateCustomerDto createCustomerDto);

        Task<IResult> UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto);

        Task<IResult> DeleteCustomerAsync(int id);
    }
}
