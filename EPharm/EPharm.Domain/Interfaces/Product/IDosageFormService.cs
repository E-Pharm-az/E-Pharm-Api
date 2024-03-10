using EPharm.Domain.Dtos.ProductDtos.DosageFormDto;

namespace EPharm.Domain.Interfaces.Product;

public interface IDosageFormService
{
    Task<IEnumerable<GetDosageFormDto>> GetAllDosageFormsAsync();
    Task<GetDosageFormDto?> GetDosageFormByIdAsync(int id);
    Task<GetDosageFormDto> CreateDosageFormAsync(CreateDosageFormDto dosageFormDto);
    Task<bool> UpdateDosageFormAsync(int id, CreateDosageFormDto dosageFormDto);
    Task<bool> DeleteDosageFormAsync(int id);
}
