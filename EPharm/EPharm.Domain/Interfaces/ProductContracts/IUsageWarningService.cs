using EPharm.Domain.Dtos.AttributeDtos;

namespace EPharm.Domain.Interfaces.ProductContracts;

public interface IUsageWarningService
{
    Task<IEnumerable<GetAttributeDto>> GetAllUsageWarningsAsync();
    Task<GetAttributeDto?> GetUsageWarningByIdAsync(int id);
    Task<GetAttributeDto> CreateUsageWarningAsync(CreateAttributeDto usageWarningDto);
    Task<bool> UpdateUsageWarningAsync(int id, CreateAttributeDto usageWarningDto);
    Task<bool> DeleteUsageWarningAsync(int id);
}
