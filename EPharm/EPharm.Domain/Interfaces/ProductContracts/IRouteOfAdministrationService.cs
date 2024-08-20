using EPharm.Domain.Dtos.AttributeDtos;

namespace EPharm.Domain.Interfaces.ProductContracts;

public interface IRouteOfAdministrationService
{
    Task<IEnumerable<GetAttributeDto>> GetAllRouteOfAdministrationsAsync();
    Task<GetAttributeDto?> GetRouteOfAdministrationByIdAsync(int id);
    Task<GetAttributeDto> CreateRouteOfAdministrationAsync(CreateAttributeDto routeOfAdministrationDto);
    Task<bool> UpdateRouteOfAdministrationAsync(int id, CreateAttributeDto routeOfAdministrationDto);
    Task<bool> DeleteRouteOfAdministrationAsync(int id);
}
