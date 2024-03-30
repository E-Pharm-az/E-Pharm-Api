using EPharm.Domain.Dtos.ActiveIngredientDto;

namespace EPharm.Domain.Interfaces.ProductContracts;

public interface IActiveIngredientService
{
    Task<IEnumerable<GetActiveIngredientDto>> GetAllActiveIngredientsAsync();
    Task<IEnumerable<GetActiveIngredientDto>> GetAllCompanyActiveIngredientsAsync(int pharmaCompanyId);
    Task<GetActiveIngredientDto?> GetActiveIngredientByIdAsync(int id);
    Task<GetActiveIngredientDto> CreateActiveIngredientAsync(int pharmaCompanyId, CreateActiveIngredientDto createActiveIngredientDto);
    Task<bool> UpdateActiveIngredientAsync(int id, CreateActiveIngredientDto createActiveIngredientDto);
    Task<bool> DeleteActiveIngredientAsync(int id);
}
