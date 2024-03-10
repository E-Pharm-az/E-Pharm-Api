using AutoMapper;
using EPharm.Domain.Dtos.ProductDtos.RouteOfAdministrationDto;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class RouteOfAdministrationProfile : Profile
{
    public RouteOfAdministrationProfile()
    {
        CreateMap<CreateRouteOfAdministrationDto, RouteOfAdministration>();
        CreateMap<RouteOfAdministration, GetRouteOfAdministrationDto>();
    }
}
