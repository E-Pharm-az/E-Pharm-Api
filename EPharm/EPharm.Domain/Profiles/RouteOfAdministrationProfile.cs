using AutoMapper;
using EPharm.Domain.Dtos.RouteOfAdministrationDto;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Domain.Profiles;

public class RouteOfAdministrationProfile : Profile
{
    public RouteOfAdministrationProfile()
    {
        CreateMap<CreateRouteOfAdministrationDto, RouteOfAdministration>();
        CreateMap<RouteOfAdministration, GetRouteOfAdministrationDto>();
        CreateMap<ProductRouteOfAdministration, GetRouteOfAdministrationDto>()
            .IncludeMembers(src => src.RouteOfAdministration);
    }
}