using AutoMapper;
using EPharm.Domain.Dtos.RegulatoryInformationDto;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class RegulatoryInformationProfile : Profile
{
public RegulatoryInformationProfile()
    {
        CreateMap<RegulatoryInformation,GetRegulatoryInformationDto>();
        CreateMap<CreateRegulatoryInformationDto, RegulatoryInformation>();
    }
}