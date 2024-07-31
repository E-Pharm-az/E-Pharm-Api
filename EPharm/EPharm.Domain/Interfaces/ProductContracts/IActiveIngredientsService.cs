using EPharm.Domain.Dtos.ActiveIngredientDto;

namespace EPharm.Domain.Interfaces.ProductContracts;

public interface IActiveIngredientsService
{
    public Task<IEnumerable<GetActiveIngredientDto>> GetAllActiveIngredientsAsync();
    public Task<IEnumerable<GetActiveIngredientDto>> GetAllCompanyActiveIngredientsAsync(int pharmaCompanyId);
    public Task<IEnumerable<GetProductActiveIngredientDto>> GetAllProductActiveIngredientsAsync(int productId);
    public Task<GetActiveIngredientDto?> GetActiveIngredientByIdAsync(int id);
    public Task<GetActiveIngredientDto> CreateActiveIngredientAsync(int pharmacyId, CreateActiveIngredientDto createActiveIngredientDto);
    public Task<bool> UpdateActiveIngredientAsync(int id, CreateActiveIngredientDto createActiveIngredientDto);
    public Task<bool> DeleteActiveIngredientAsync(int id);
}