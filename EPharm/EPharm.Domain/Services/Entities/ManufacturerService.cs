using AutoMapper;
using EPharm.Domain.Dtos.ManufacturerDto;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;

namespace EPharm.Domain.Services.Entities;

public class ManufacturerService(IManufacturerRepository manufacturerRepository, IMapper mapper) : IManufacturerService
{
    public async Task<IEnumerable<GetManufacturerDto>> GetAllCompanyManufacturersAsync(int pharmaCompanyId)
    {
        var manufacturers = await manufacturerRepository.GetAllCompanyManufacturersAsync(pharmaCompanyId);
        return mapper.Map<IEnumerable<GetManufacturerDto>>(manufacturers);
    }

    public async Task<GetManufacturerDto?> GetManufacturerByIdAsync(int id)
    {
        var manufacturer = await manufacturerRepository.GetByIdAsync(id);
        return mapper.Map<GetManufacturerDto>(manufacturer);
    }

    public async Task<GetManufacturerDto> CreateManufacturerAsync(int pharmaCompanyId,
        CreateManufacturerDto createManufacturerDto)
    {
        try
        {
            var manufacturerEntity = mapper.Map<Manufacturer>(createManufacturerDto);
            manufacturerEntity.PharmacyId = pharmaCompanyId;
            var manufacturer = await manufacturerRepository.InsertAsync(manufacturerEntity);

            return mapper.Map<GetManufacturerDto>(manufacturer);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create manufacturer. Details: {ex.Message}");
        }
    }

    public async Task<bool> UpdateManufacturerAsync(int id, CreateManufacturerDto createManufacturerDto)
    {
        var manufacturer = await manufacturerRepository.GetByIdAsync(id);

        if (manufacturer is null)
            return false;

        mapper.Map(createManufacturerDto, manufacturer);
        manufacturerRepository.Update(manufacturer);

        var result = await manufacturerRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteManufacturerAsync(int id)
    {
        var manufacturer = await manufacturerRepository.GetByIdAsync(id);

        if (manufacturer is null)
            return false;

        manufacturerRepository.Delete(manufacturer);

        var result = await manufacturerRepository.SaveChangesAsync();
        return result > 0;
    }
}