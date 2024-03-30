using EPharm.Domain.Dtos.DosageFormDto;

namespace EPharm.Domain.Interfaces.ProductContracts;

public interface IDosageFormService
{
    Task<IEnumerable<GetDosageFormDto>> GetAllDosageFormsAsync();
    Task<GetDosageFormDto?> GetDosageFormByIdAsync(int id);
    Task<GetDosageFormDto> CreateDosageFormAsync(CreateDosageFormDto dosageFormDto);
    Task<bool> UpdateDosageFormAsync(int id, CreateDosageFormDto dosageFormDto);
    Task<bool> DeleteDosageFormAsync(int id);
}
