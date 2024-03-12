using AutoMapper;
using EPharm.Domain.Dtos.UsageWarningDto;
using EPharm.Domain.Interfaces.Product;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Domain.Services.ProductServices;

public class UsageWarningService(IUsageWarningRepository usageWarningRepository, IMapper mapper) : IUsageWarningService
{
    public async Task<IEnumerable<GetUsageWarningDto>> GetAllUsageWarningsAsync()
    {
        var usageWarnings = await usageWarningRepository.GetAllAsync();
        return mapper.Map<IEnumerable<GetUsageWarningDto>>(usageWarnings);
    }

    public async Task<GetUsageWarningDto?> GetUsageWarningByIdAsync(int id)
    {
        var usageWarning = await usageWarningRepository.GetByIdAsync(id);
        return mapper.Map<GetUsageWarningDto>(usageWarning);
    }

    public async Task<GetUsageWarningDto> CreateUsageWarningAsync(CreateUsageWarningDto usageWarningDto)
    {
        try
        {
            var usageWarningEntity = mapper.Map<UsageWarning>(usageWarningDto);
            var usageWarning = await usageWarningRepository.InsertAsync(usageWarningEntity);

            return mapper.Map<GetUsageWarningDto>(usageWarning);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create usage warning. Details: {ex.Message}");
        }
    }

    public async Task<bool> UpdateUsageWarningAsync(int id, CreateUsageWarningDto usageWarningDto)
    {
        var usageWarning = await usageWarningRepository.GetByIdAsync(id);

        if (usageWarning is null)
            return false;

        mapper.Map(usageWarningDto, usageWarning);
        usageWarningRepository.Update(usageWarning);

        var result = await usageWarningRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteUsageWarningAsync(int id)
    {
        var usageWarning = await usageWarningRepository.GetByIdAsync(id);

        if (usageWarning is null)
            return false;

        usageWarningRepository.Delete(usageWarning);
        var result = await usageWarningRepository.SaveChangesAsync();
        return result > 0;
    }
}
