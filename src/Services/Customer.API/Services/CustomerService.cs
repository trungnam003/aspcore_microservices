using AutoMapper;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Customer;

namespace Customer.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        public CustomerService(ICustomerRepository repository, IMapper mapper) {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IResult> GetCustomerByUsernameAsync(string username)
        {
            var result = await _repository.GetCustomerByUsernameAsync(username);
            if(result == null)
            {
                return Results.NotFound($"Customer with username {username} not found");
            }
            var customerDto = _mapper.Map<CustomerDto>(result);
            return Results.Ok(customerDto);
        }
        public async Task<IResult> GetAllCustomer()
        {
            var resulls = await _repository.GetAllCustomer();
            var customersDto = _mapper.Map<IEnumerable<CustomerDto>>(resulls);
            return Results.Ok(customersDto);
        }

        public async Task<IResult> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            var alreadyExists = await _repository.GetCustomerByUsernameAsync(createCustomerDto.UserName);
            if(alreadyExists != null)
            {
                return Results.BadRequest("Customer already exists");
            }
            var customer = _mapper.Map<Entities.Customer>(createCustomerDto);
            await _repository.CreateCustomerAsync(customer);
            await _repository.SaveAsync();
            return Results.Created($"/api/customers/{customer.Id}", createCustomerDto);
        }

        public async Task<IResult> UpdateCustomerAsync(int id,UpdateCustomerDto updateCustomerDto)
        {
            var customer = await _repository.GetByIdAsync(id);
            if(customer == null)
            {
                return Results.NotFound($"Customer with id {id} not found");
            }
            _mapper.Map(updateCustomerDto, customer);
            await _repository.UpdateCustomerAsync(customer);
            await _repository.SaveAsync();
            return Results.NoContent();
        }

        public async Task<IResult> DeleteCustomerAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if(customer == null)
            {
                return Results.NotFound($"Customer with id {id} not found");
            }
            await _repository.DeleteAsync(customer);
            return Results.NoContent();
        }
    }
}
