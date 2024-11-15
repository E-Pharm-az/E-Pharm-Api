using AutoMapper;
using EPharm.Domain.Dtos.SpecialRequirementsDto;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class SpecialRequirementProfile : Profile
{
    public SpecialRequirementProfile()
    {
        CreateMap<CreateSpecialRequirementDto, SpecialRequirement>();
        CreateMap<SpecialRequirement, GetSpecialRequirementDto>();
    }
}