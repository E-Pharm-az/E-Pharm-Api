using AutoMapper;
using EPharm.Domain.Dtos.PharmaCompanyDtos;
using EPharm.Infrastructure.Context.Entities;

namespace EPharm.Domain.Profiles;

public class PharmaCompanyProfile : Profile
{
    public PharmaCompanyProfile()
    {
        CreateMap<PharmaCompany, GetPharmaCompanyDto>();
        CreateMap<CreatePharmaCompanyDto, PharmaCompany>();
    } 
}
