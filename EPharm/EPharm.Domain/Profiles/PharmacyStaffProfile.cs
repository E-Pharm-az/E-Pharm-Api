using AutoMapper;
using EPharm.Domain.Dtos.PharmacyStaffDto;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Infrastructure.Entities.PharmaEntities;

namespace EPharm.Domain.Profiles;

public class PharmacyStaffProfile : Profile
{
    public PharmacyStaffProfile()
    {
        CreateMap<PharmacyStaff, GetPharmacyStaffDto>();
        CreateMap<CreatePharmacyStaffDto, PharmacyStaff>();
        CreateMap<GetUserDto, CreatePharmacyStaffDto>();
    }
}