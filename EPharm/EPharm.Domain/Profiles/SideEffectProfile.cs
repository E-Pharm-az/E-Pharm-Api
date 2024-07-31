using AutoMapper;
using EPharm.Domain.Dtos.SideEffectDto;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class SideEffectProfile : Profile
{
    public SideEffectProfile()
    {
        CreateMap<CreateSideEffectDto, SideEffect>();
        CreateMap<SideEffect, GetSideEffectDto>();
        CreateMap<ProductSideEffect, GetSideEffectDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SideEffect.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SideEffect.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.SideEffect.Description));
    }
}