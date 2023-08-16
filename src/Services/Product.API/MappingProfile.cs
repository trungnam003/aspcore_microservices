using AutoMapper;
using Infrastructure.Mapping;
using Product.API.Entities;
using Shared.DTOs.Product;

namespace Product.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CatalogProduct, ProductDto>().ReverseMap();
            CreateMap<CatalogProduct, CreateProductDto>().ReverseMap();
            CreateMap<CatalogProduct, UpdateProductDto>().ReverseMap()
                .IgnoreAllNonExisting();
        }
    }

}
