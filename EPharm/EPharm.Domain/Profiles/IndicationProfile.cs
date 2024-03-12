using AutoMapper;
using EPharm.Domain.Dtos.IndicationDto;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class IndicationProfile : Profile
{
    public IndicationProfile()
    {
        CreateMap<CreateIndicationDto, Indication>();
        CreateMap<Indication, GetIndicationDto>();
    }
}
