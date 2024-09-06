using AutoMapper;
using EPharm.Domain.Dtos.ProductDtos;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.ManufacturingDate, opt => opt.MapFrom(src => src.ManufacturingDate.ToUniversalTime()))
            .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate.ToUniversalTime()));
        CreateMap<Product, GetProductDto>()
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock.Sum(p => p.Quantity)));
        CreateMap<Product, GetDetailProductDto>()
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock.Sum(p => p.Quantity)));
    }
}
