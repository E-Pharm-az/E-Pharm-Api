using AutoMapper;
using EPharm.Domain.Dtos.ActiveIngredientDto;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class ActiveIngredientProfile : Profile
{
    public ActiveIngredientProfile()
    {
        CreateMap<CreateActiveIngredientDto, ActiveIngredient>();
        CreateMap<ActiveIngredient, GetActiveIngredientDto>();
        CreateMap<ProductActiveIngredient, GetActiveIngredientDto>().IncludeMembers(src => src.ActiveIngredient);        
    }
}
