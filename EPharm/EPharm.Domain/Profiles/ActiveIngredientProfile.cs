using AutoMapper;
using EPharm.Domain.Dtos.ActiveIngredientDto;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class ActiveIngredientProfile : Profile
{
    public ActiveIngredientProfile()
    {
        CreateMap<ActiveIngredient, GetActiveIngredientDto>();
        CreateMap<CreateActiveIngredientDto, ActiveIngredient>();
    }
}
