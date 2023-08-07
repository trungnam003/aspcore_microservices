using AutoMapper;
using Shared.DTOs.Customer;

namespace Customer.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Customer, CustomerDto>().ReverseMap();
            CreateMap<Entities.Customer, CreateCustomerDto>().ReverseMap();
            CreateMap<Entities.Customer, UpdateCustomerDto>().ReverseMap();
        }
    }
}
