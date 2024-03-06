using EPharm.Domain.Dtos.ProductDtos.ActiveIngredientDto;

namespace EPharm.Domain.Interfaces.Product;

public interface IActiveIngredientService
{
    Task<List<GetActiveIngredientDto>> GetAllActiveIngredientsAsync();
    Task<GetActiveIngredientDto?> GetActiveIngredientByIdAsync(int id);
    Task<GetActiveIngredientDto> CreateActiveIngredientAsync(CreateActiveIngredientDto createActiveIngredientDto);
    Task<bool> UpdateActiveIngredientAsync(int id, CreateActiveIngredientDto createActiveIngredientDto);
    Task<bool> DeleteActiveIngredientAsync(int id);
}
