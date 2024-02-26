using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class ProductActiveIngredientRepository : Repository<ProductActiveIngredient>, IProductActiveIngredientRepository
{
    protected ProductActiveIngredientRepository(AppDbContext context) : base(context)
    {
    }
}
