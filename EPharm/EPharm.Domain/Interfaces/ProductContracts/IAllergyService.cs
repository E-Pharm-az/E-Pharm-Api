using EPharm.Domain.Dtos.AttributeDtos;

namespace EPharm.Domain.Interfaces.ProductContracts;

public interface IAllergyService
{
    Task<IEnumerable<GetAttributeDto>> GetAllAllergiesAsync();
    Task<GetAttributeDto?> GetAllergyByIdAsync(int id);
    public Task<GetAttributeDto> CreateAllergyAsync(CreateAttributeDto allergyDto);
    public Task<bool> UpdateAllergyAsync(int id, CreateAttributeDto allergyDto);
    Task<bool> DeleteAllergyAsync(int id);
}
