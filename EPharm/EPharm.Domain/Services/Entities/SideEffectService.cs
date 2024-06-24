using AutoMapper;
using EPharm.Domain.Dtos.SideEffectDto;
using EPharm.Domain.Interfaces.ProductContracts;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;

namespace EPharm.Domain.Services.Entities;

public class SideEffectService(ISideEffectRepository sideEffectRepository, IMapper mapper) : ISideEffectService
{
    public async Task<IEnumerable<GetSideEffectDto>> GetAllSideEffectsAsync()
    {
        var sideEffects = await sideEffectRepository.GetAllAsync();
        return mapper.Map<IEnumerable<GetSideEffectDto>>(sideEffects);
    }

    public async Task<GetSideEffectDto?> GetSideEffectByIdAsync(int id)
    {
        var sideEffect = await sideEffectRepository.GetByIdAsync(id);
        return mapper.Map<GetSideEffectDto>(sideEffect);
    }

    public async Task<GetSideEffectDto> CreateSideEffectAsync(CreateSideEffectDto sideEffectDto)
    {
        try
        {
            var sideEffectEntity = mapper.Map<SideEffect>(sideEffectDto);
            var sideEffect = await sideEffectRepository.InsertAsync(sideEffectEntity);

            return mapper.Map<GetSideEffectDto>(sideEffect);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create side effect. Details: {ex.Message}");
        }
    }

    public async Task<bool> UpdateSideEffectAsync(int id, CreateSideEffectDto sideEffectDto)
    {
        var sideEffect = await sideEffectRepository.GetByIdAsync(id);

        if (sideEffect is null)
            return false;

        mapper.Map(sideEffectDto, sideEffect);
        sideEffectRepository.Update(sideEffect);

        var result = await sideEffectRepository.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteSideEffectAsync(int id)
    {
        var sideEffect = await sideEffectRepository.GetByIdAsync(id);

        if (sideEffect is null)
            return false;

        sideEffectRepository.Delete(sideEffect);
        var result = await sideEffectRepository.SaveChangesAsync();
        return result > 0;
    }
}