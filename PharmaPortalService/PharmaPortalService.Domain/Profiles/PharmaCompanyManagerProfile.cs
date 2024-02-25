using AutoMapper;
using PharmaPortalService.Domain.Dtos.PharmaCompanyManagerDto;
using PharmaPortalService.Infrastructure.Context.Entities;

namespace PharmaPortalService.Domain.Profiles;

public class PharmaCompanyManagerProfile : Profile
{
    public PharmaCompanyManagerProfile()
    {
        CreateMap<PharmaCompanyManager, GetPharmaCompanyManagerDto>();
        CreateMap<CreatePharmaCompanyManagerDto, PharmaCompanyManager>();
    }
}
