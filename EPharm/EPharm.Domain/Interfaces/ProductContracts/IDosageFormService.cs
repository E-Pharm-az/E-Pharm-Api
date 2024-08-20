using EPharm.Domain.Dtos.AttributeDtos;

namespace EPharm.Domain.Interfaces.ProductContracts;

public interface IDosageFormService
{
    Task<IEnumerable<GetAttributeDto>> GetAllDosageFormsAsync();
    Task<GetAttributeDto?> GetDosageFormByIdAsync(int id);
    public Task<GetAttributeDto> CreateDosageFormAsync(CreateAttributeDto dosageFormDto);
    public Task<bool> UpdateDosageFormAsync(int id, CreateAttributeDto dosageFormDto);
    Task<bool> DeleteDosageFormAsync(int id);
}
