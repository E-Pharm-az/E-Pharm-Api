using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Junctions;

public interface IProductActiveIngredientRepository : IRepository<ProductActiveIngredient>
{
    public Task InsertProductActiveIngredientAsync(int productId, int[] activeIngredientsIds);
}
