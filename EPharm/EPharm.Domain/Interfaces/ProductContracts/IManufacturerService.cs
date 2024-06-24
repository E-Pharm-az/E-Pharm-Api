using EPharm.Domain.Dtos.ManufacturerDto;

namespace EPharm.Domain.Interfaces.ProductContracts;

public interface IManufacturerService
{
    Task<IEnumerable<GetManufacturerDto>> GetAllCompanyManufacturersAsync(int pharmaCompanyId);
    Task<GetManufacturerDto?> GetManufacturerByIdAsync(int id);
    Task<GetManufacturerDto> CreateManufacturerAsync(int pharmaCompanyId, CreateManufacturerDto manufacturer);
    Task<bool> UpdateManufacturerAsync(int id, CreateManufacturerDto manufacturer);
    Task<bool> DeleteManufacturerAsync(int id);
}