using Customer.API.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Customer;
using System;

namespace Customer.API.Controllers
{
    public static class CustomerController
    {
        public static void MapCustomerController(this WebApplication app)
        {
            app.MapGet("/api/customers", async (ICustomerService customerService) =>
            {
                return await customerService.GetAllCustomer();
            });

            app.MapGet("/api/customers/{username}", async (ICustomerService customerService, [FromRoute] string username) =>
            {
                return await customerService.GetCustomerByUsernameAsync(username);
            });

            app.MapPost("/api/customers", async (ICustomerService customerService, CreateCustomerDto customerDto) =>
            {
                if (!customerDto.IsValid())
                {
                    return Results.BadRequest(customerDto.GetValidationResult());
                }

                return await customerService.CreateCustomerAsync(customerDto);
            });

            app.MapPut("/api/customers/{id:int}", async (ICustomerService customerService, IValidator<UpdateCustomerDto> validator, [FromRoute] int id, UpdateCustomerDto customerDto) =>
            {
                var validationResult = await validator.ValidateAsync(customerDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult);
                }
                return await customerService.UpdateCustomerAsync(id, customerDto);
            });
            
            app.MapDelete("/api/customers/{id:int}", async (ICustomerService customerService, [FromRoute] int id) =>
            {
                return await customerService.DeleteCustomerAsync(id);
            });
        }
    }
}
