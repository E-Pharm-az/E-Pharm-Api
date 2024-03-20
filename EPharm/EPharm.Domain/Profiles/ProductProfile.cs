using AutoMapper;
using EPharm.Domain.Dtos.ProductDtos;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductDto, Product>();
        CreateMap<Product, GetProductDto>();
        CreateMap<Product, GetFullProductDto>();
    }
}
