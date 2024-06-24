using AutoMapper;
using EPharm.Domain.Dtos.PharmaCompanyDtos;
using EPharm.Infrastructure.Entities.PharmaEntities;

namespace EPharm.Domain.Profiles;

public class PharmaCompanyProfile : Profile
{
    public PharmaCompanyProfile()
    {
        CreateMap<Pharmacy, GetPharmaCompanyDto>();
        CreateMap<CreatePharmaCompanyDto, Pharmacy>();
    }
}