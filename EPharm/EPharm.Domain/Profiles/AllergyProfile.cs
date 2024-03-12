using AutoMapper;
using EPharm.Domain.Dtos.AllergyDto;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class AllergyProfile : Profile
{
    public AllergyProfile()
    {
        CreateMap<CreateAllergyDto, Allergy>();
        CreateMap<Allergy, GetAllergyDto>();
    }
}
