using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

namespace EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

public interface IProductActiveIngredientRepository : IRepository<ProductActiveIngredient>
{
    public Task InsertProductActiveIngredientAsync(int productId, int[] activeIngredientsIds);
}
