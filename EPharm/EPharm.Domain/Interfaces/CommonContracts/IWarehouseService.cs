using EPharm.Domain.Dtos.WarehouseDto;

namespace EPharm.Domain.Interfaces.CommonContracts;

public interface IWarehouseService
{
    public Task<IEnumerable<GetWarehouseDto>> GetAllCompanyWarehousesAsync(int pharmaCompanyId);
    public Task<GetWarehouseDto?> GetWarehouseByIdAsync(int id);
    public Task<GetWarehouseDto> CreateWarehouseAsync(int pharmaCompanyId, CreateWarehouseDto warehouseDto);
    public Task<bool> UpdateWarehouseAsync(int id, CreateWarehouseDto warehouse);
    public Task<bool> DeleteWarehouseAsync(int id);
}
