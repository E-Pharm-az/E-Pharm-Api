using AutoMapper;
using EPharm.Domain.Dtos.ProductDtos.RouteOfAdministrationDto;
using EPharm.Domain.Interfaces.Product;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Domain.Services.ProductServices;

public class RouteOfAdministrationService(IRouteOfAdministrationRepository routeOfAdministrationRepository, IMapper mapper) : IRouteOfAdministrationService
{
    public async Task<IEnumerable<GetRouteOfAdministrationDto>> GetAllRouteOfAdministrationsAsync()
    {
        var routeOfAdministrations = await routeOfAdministrationRepository.GetAllAsync();
        return mapper.Map<IEnumerable<GetRouteOfAdministrationDto>>(routeOfAdministrations);
    }

    public async Task<GetRouteOfAdministrationDto?> GetRouteOfAdministrationByIdAsync(int id)
    {
        var routeOfAdministration = await routeOfAdministrationRepository.GetByIdAsync(id);
        return mapper.Map<GetRouteOfAdministrationDto>(routeOfAdministration);
    }

    public async Task<GetRouteOfAdministrationDto> CreateRouteOfAdministrationAsync(CreateRouteOfAdministrationDto routeOfAdministrationDto)
    {
        try
        {
            var routeOfAdministrationEntity = mapper.Map<RouteOfAdministration>(routeOfAdministrationDto);
            var routeOfAdministration = await routeOfAdministrationRepository.InsertAsync(routeOfAdministrationEntity);

            return mapper.Map<GetRouteOfAdministrationDto>(routeOfAdministration);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create route of administration. Details: {ex.Message}");
        }
    }

    public async Task<bool> UpdateRouteOfAdministrationAsync(int id, CreateRouteOfAdministrationDto routeOfAdministrationDto)
    {
        var routeOfAdministration = await routeOfAdministrationRepository.GetByIdAsync(id);

        if (routeOfAdministration is null)
            return false;

        mapper.Map(routeOfAdministrationDto, routeOfAdministration);
        routeOfAdministrationRepository.Update(routeOfAdministration);

        var result = await routeOfAdministrationRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteRouteOfAdministrationAsync(int id)
    {
        var routeOfAdministration = await routeOfAdministrationRepository.GetByIdAsync(id);

        if (routeOfAdministration is null)
            return false;

        routeOfAdministrationRepository.Delete(routeOfAdministration);
        var result = await routeOfAdministrationRepository.SaveChangesAsync();
        return result > 0;
    }
}
