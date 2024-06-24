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
        CreateMap<IndicationProduct, GetIndicationDto>().IncludeMembers(src => src.Indication);
    }
}