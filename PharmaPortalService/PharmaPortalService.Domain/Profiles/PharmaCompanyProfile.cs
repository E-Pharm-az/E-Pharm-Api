using AutoMapper;
using PharmaPortalService.Domain.Dtos.PharmaCompanyDtos;
using PharmaPortalService.Infrastructure.Context.Entities;

namespace PharmaPortalService.Domain.Profiles;

public class PharmaCompanyProfile : Profile
{
    public PharmaCompanyProfile()
    {
        CreateMap<PharmaCompany, GetPharmaCompanyDto>();
        CreateMap<CreatePharmaCompanyDto, PharmaCompany>();
    } 
}
