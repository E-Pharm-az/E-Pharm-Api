using EPharm.Domain.Dtos.RouteOfAdministrationDto;

namespace EPharm.Domain.Interfaces.Product;

public interface IRouteOfAdministrationService
{
    Task<IEnumerable<GetRouteOfAdministrationDto>> GetAllRouteOfAdministrationsAsync();
    Task<GetRouteOfAdministrationDto?> GetRouteOfAdministrationByIdAsync(int id);
    Task<GetRouteOfAdministrationDto> CreateRouteOfAdministrationAsync(CreateRouteOfAdministrationDto routeOfAdministrationDto);
    Task<bool> UpdateRouteOfAdministrationAsync(int id, CreateRouteOfAdministrationDto routeOfAdministrationDto);
    Task<bool> DeleteRouteOfAdministrationAsync(int id);
}
