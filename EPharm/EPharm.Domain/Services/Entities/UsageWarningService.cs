using AutoMapper;
using EPharm.Domain.Dtos.AttributeDtos;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;

namespace EPharm.Domain.Services.Entities;

public class UsageWarningService(IUsageWarningRepository usageWarningRepository, IMapper mapper) : IUsageWarningService
{
    public async Task<IEnumerable<GetAttributeDto>> GetAllUsageWarningsAsync()
    {
        var usageWarnings = await usageWarningRepository.GetAllAsync();
        return mapper.Map<IEnumerable<GetAttributeDto>>(usageWarnings);
    }

    public async Task<GetAttributeDto?> GetUsageWarningByIdAsync(int id)
    {
        var usageWarning = await usageWarningRepository.GetByIdAsync(id);
        return mapper.Map<GetAttributeDto>(usageWarning);
    }

    public async Task<GetAttributeDto> CreateUsageWarningAsync(CreateAttributeDto usageWarningDto)
    {
        try
        {
            var usageWarningEntity = mapper.Map<UsageWarning>(usageWarningDto);
            var usageWarning = await usageWarningRepository.InsertAsync(usageWarningEntity);

            return mapper.Map<GetAttributeDto>(usageWarning);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create usage warning. Details: {ex.Message}");
        }
    }

    public async Task<bool> UpdateUsageWarningAsync(int id, CreateAttributeDto usageWarningDto)
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
