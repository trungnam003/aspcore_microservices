using AutoMapper;
using Shared.DTOs.Product;
using Product.API.Entities;
using Infrastructure.Mapping;

namespace Product.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CatalogProduct, ProductDto>();
            CreateMap<CatalogProduct, CreateProductDto>().ReverseMap();
            CreateMap<CatalogProduct, UpdateProductDto>().ReverseMap()
                .IgnoreAllNonExisting();
        }
    }
    
}
