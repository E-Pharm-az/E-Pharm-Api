using EPharm.Domain.Dtos.AttributeDtos;

namespace EPharm.Domain.Interfaces.ProductContracts;

public interface ISideEffectService
{
    Task<IEnumerable<GetAttributeDto>> GetAllSideEffectsAsync();
    Task<GetAttributeDto?> GetSideEffectByIdAsync(int id);
    Task<GetAttributeDto> CreateSideEffectAsync(CreateAttributeDto sideEffectDto);
    Task<bool> UpdateSideEffectAsync(int id, CreateAttributeDto sideEffectDto);
    Task<bool> DeleteSideEffectAsync(int id);
}
