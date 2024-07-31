using AutoMapper;
using EPharm.Domain.Dtos.ActiveIngredientDto;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class ActiveIngredientProfile : Profile
{
    public ActiveIngredientProfile()
    {
        CreateMap<CreateActiveIngredientDto, ActiveIngredient>();
        CreateMap<ActiveIngredient, GetActiveIngredientDto>();
        CreateMap<ProductActiveIngredient, GetProductActiveIngredientDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ActiveIngredient.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ActiveIngredient.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ActiveIngredient.Description))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ActiveIngredient.PharmacyId));
    }
}