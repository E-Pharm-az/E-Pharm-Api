using AutoMapper;
using EPharm.Domain.Dtos.AttributeDtos;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;

namespace EPharm.Domain.Services.Entities;

public class RouteOfAdministrationService(
    IRouteOfAdministrationRepository routeOfAdministrationRepository,
    IMapper mapper) : IRouteOfAdministrationService
{
    public async Task<IEnumerable<GetAttributeDto>> GetAllRouteOfAdministrationsAsync()
    {
        var routeOfAdministrations = await routeOfAdministrationRepository.GetAllAsync();
        return mapper.Map<IEnumerable<GetAttributeDto>>(routeOfAdministrations);
    }

    public async Task<GetAttributeDto?> GetRouteOfAdministrationByIdAsync(int id)
    {
        var routeOfAdministration = await routeOfAdministrationRepository.GetByIdAsync(id);
        return mapper.Map<GetAttributeDto>(routeOfAdministration);
    }

    public async Task<GetAttributeDto> CreateRouteOfAdministrationAsync(CreateAttributeDto routeOfAdministrationDto)
    {
        try
        {
            var routeOfAdministrationEntity = mapper.Map<RouteOfAdministration>(routeOfAdministrationDto);
            var routeOfAdministration = await routeOfAdministrationRepository.InsertAsync(routeOfAdministrationEntity);

            return mapper.Map<GetAttributeDto>(routeOfAdministration);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create route of administration. Details: {ex.Message}");
        }
    }

    public async Task<bool> UpdateRouteOfAdministrationAsync(int id, CreateAttributeDto routeOfAdministrationDto)
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
