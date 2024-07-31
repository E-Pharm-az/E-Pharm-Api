using AutoMapper;
using EPharm.Domain.Dtos.AllergyDto;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class AllergyProfile : Profile
{
    public AllergyProfile()
    {
        CreateMap<CreateAllergyDto, Allergy>();
        CreateMap<Allergy, GetAllergyDto>();
        CreateMap<ProductAllergy, GetAllergyDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Allergy.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Allergy.Name));
    }
}