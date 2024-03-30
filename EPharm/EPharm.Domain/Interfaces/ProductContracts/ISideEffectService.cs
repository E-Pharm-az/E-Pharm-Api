using EPharm.Domain.Dtos.SideEffectDto;

namespace EPharm.Domain.Interfaces.ProductContracts;

public interface ISideEffectService
{
    Task<IEnumerable<GetSideEffectDto>> GetAllSideEffectsAsync();
    Task<GetSideEffectDto?> GetSideEffectByIdAsync(int id);
    Task<GetSideEffectDto> CreateSideEffectAsync(CreateSideEffectDto sideEffectDto);
    Task<bool> UpdateSideEffectAsync(int id, CreateSideEffectDto sideEffectDto);
    Task<bool> DeleteSideEffectAsync(int id);
}
