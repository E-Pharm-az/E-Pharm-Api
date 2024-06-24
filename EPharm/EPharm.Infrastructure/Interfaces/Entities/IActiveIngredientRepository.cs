using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Entities;

public interface IActiveIngredientRepository : IRepository<ActiveIngredient>
{
    Task<IEnumerable<ActiveIngredient>> GetAllCompanyActiveIngredientsAsync(int pharmaCompanyId);
}
