using AutoMapper;
using EPharm.Domain.Dtos.ProductImageDto;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class ProductImageProfile : Profile
{
    public ProductImageProfile()
    {
        CreateMap<ProductImage, GetProductImageDto>();
        CreateMap<ProductImage, CreateProductImageDto>();
    }
}
