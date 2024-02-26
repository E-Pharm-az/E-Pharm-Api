using AutoMapper;
using EPharm.Domain.Dtos.PharmaCompanyManagerDto;
using EPharm.Infrastructure.Context.Entities;

namespace EPharm.Domain.Profiles;

public class PharmaCompanyManagerProfile : Profile
{
    public PharmaCompanyManagerProfile()
    {
        CreateMap<PharmaCompanyManager, GetPharmaCompanyManagerDto>();
        CreateMap<CreatePharmaCompanyManagerDto, PharmaCompanyManager>();
    }
}
