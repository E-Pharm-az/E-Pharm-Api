using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Product;

public interface IActiveIngredientRepository : IRepository<ActiveIngredient>
{
    Task<IEnumerable<ActiveIngredient>> GetAllCompanyActiveIngredientsAsync(int pharmaCompanyId);
}
