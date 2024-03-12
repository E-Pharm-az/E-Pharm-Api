using AutoMapper;
using EPharm.Domain.Dtos.ProductDtos;
using EPharm.Domain.Dtos.ProductDtos.ProductDtos;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductDto, Product>();
        CreateMap<Product, GetProductDto>()
            .ForMember(dest => dest.ProductImageUrls, opt => opt.MapFrom(src => src.ProductImages.Select(pi => pi.ImageUrl).ToArray()));
    }
}
