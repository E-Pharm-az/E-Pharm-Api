using AutoMapper;
using EPharm.Domain.Dtos.PharmacyDtos;
using EPharm.Infrastructure.Entities.PharmaEntities;

namespace EPharm.Domain.Profiles;

public class PharmacyProfile : Profile
{
    public PharmacyProfile()
    {
        CreateMap<Pharmacy, GetPharmacyDto>();
        CreateMap<CreatePharmacyDto, Pharmacy>();
    }
}