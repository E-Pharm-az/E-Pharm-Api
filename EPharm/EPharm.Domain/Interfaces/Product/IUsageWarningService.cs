using EPharm.Domain.Dtos.ProductDtos.UsageWarningDto;

namespace EPharm.Domain.Interfaces.Product;

public interface IUsageWarningService
{
    Task<IEnumerable<GetUsageWarningDto>> GetAllUsageWarningsAsync();
    Task<GetUsageWarningDto?> GetUsageWarningByIdAsync(int id);
    Task<GetUsageWarningDto> CreateUsageWarningAsync(CreateUsageWarningDto usageWarningDto);
    Task<bool> UpdateUsageWarningAsync(int id, CreateUsageWarningDto usageWarningDto);
    Task<bool> DeleteUsageWarningAsync(int id);
}
