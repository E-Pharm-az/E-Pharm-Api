using AutoMapper;
using EPharm.Domain.Dtos.ProductDtos.SpecialRequirementsDto;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class SpecialRequirementProfile : Profile
{
    public SpecialRequirementProfile()
    {
        CreateMap<CreateSpecialRequirementDto, SpecialRequirement>();
        CreateMap<SpecialRequirement, GetSpecialRequirementDto>();
    }
}
