using AutoMapper;
using EPharm.Domain.Dtos.PharmaCompanyManagerDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;

namespace EPharm.Domain.Profiles;

public class PharmaCompanyManagerProfile : Profile
{
    public PharmaCompanyManagerProfile()
    {
        CreateMap<PharmacyStaff, GetPharmaCompanyManagerDto>();
        CreateMap<CreatePharmaCompanyManagerDto, PharmacyStaff>();
        CreateMap<GetUserDto, CreatePharmaCompanyManagerDto>();
    }
}
