using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Junctions;

public interface IProductActiveIngredientRepository : IRepository<ProductActiveIngredient>
{
    public Task InsertAsync(int productId, int[] activeIngredientsIds);
}
