using AutoMapper;
using EPharm.Domain.Dtos.SideEffectDto;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class SideEffectProfile : Profile
{
    public SideEffectProfile()
    {
        CreateMap<CreateSideEffectDto, SideEffect>();
        CreateMap<SideEffect, GetSideEffectDto>();
    }
}