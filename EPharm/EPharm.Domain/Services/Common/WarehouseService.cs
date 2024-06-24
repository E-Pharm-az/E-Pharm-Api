using AutoMapper;
using EPharm.Domain.Dtos.WarehouseDto;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;

namespace EPharm.Domain.Services.Common;

public class WarehouseService(IWarehouseRepository warehouseRepository, IMapper mapper) : IWarehouseService
{
    public async Task<IEnumerable<GetWarehouseDto>> GetAllCompanyWarehousesAsync(int pharmaCompanyId)
    {
        var warehouses = await warehouseRepository.GetAllCompanyWarehousesAsync(pharmaCompanyId);
        return mapper.Map<IEnumerable<GetWarehouseDto>>(warehouses);
    }

    public async Task<GetWarehouseDto?> GetWarehouseByIdAsync(int id)
    {
        var warehouse = await warehouseRepository.GetByIdAsync(id);
        return mapper.Map<GetWarehouseDto>(warehouse);
    }

    public async Task<GetWarehouseDto> CreateWarehouseAsync(int pharmaCompanyId, CreateWarehouseDto warehouseDto)
    {
        try
        {
            var warehouseEntity = mapper.Map<Warehouse>(warehouseDto);
            warehouseEntity.PharmaCompanyId = pharmaCompanyId;
            var warehouse = await warehouseRepository.InsertAsync(warehouseEntity);

            return mapper.Map<GetWarehouseDto>(warehouse);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create warehouse. Details: {ex.Message}");
        }
    }

    public async Task<bool> UpdateWarehouseAsync(int id, CreateWarehouseDto warehouse)
    {
        var warehouseEntity = await warehouseRepository.GetByIdAsync(id);

        if (warehouseEntity is null)
            return false;

        mapper.Map(warehouse, warehouseEntity);
        warehouseRepository.Update(warehouseEntity);

        var result = await warehouseRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteWarehouseAsync(int id)
    {
        var warehouse = await warehouseRepository.GetByIdAsync(id);

        if (warehouse is null)
            return false;

        warehouseRepository.Delete(warehouse);

        var result = await warehouseRepository.SaveChangesAsync();
        return result > 0;
    }
}