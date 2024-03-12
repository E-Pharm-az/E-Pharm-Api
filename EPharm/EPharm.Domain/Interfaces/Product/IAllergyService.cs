using EPharm.Domain.Dtos.AllergyDto;

namespace EPharm.Domain.Interfaces.Product;

public interface IAllergyService
{
    Task<IEnumerable<GetAllergyDto>> GetAllAllergiesAsync();
    Task<GetAllergyDto?> GetAllergyByIdAsync(int id);
    Task<GetAllergyDto> CreateAllergyAsync(CreateAllergyDto allergyDto);
    Task<bool> UpdateAllergyAsync(int id, CreateAllergyDto allergyDto);
    Task<bool> DeleteAllergyAsync(int id);
}
