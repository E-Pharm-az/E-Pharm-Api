using AutoMapper;
using EPharm.Domain.Dtos.IndicationDto;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class IndicationProfile : Profile
{
    public IndicationProfile()
    {
        CreateMap<CreateIndicationDto, Indication>();
        CreateMap<Indication, GetIndicationDto>();
        CreateMap<IndicationProduct, GetIndicationDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Indication.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Indication.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Indication.Description)); 
    }
}