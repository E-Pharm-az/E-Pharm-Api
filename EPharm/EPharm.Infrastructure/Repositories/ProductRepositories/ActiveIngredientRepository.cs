using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class ActiveIngredientRepository : Repository<ActiveIngredient>, IActiveIngredientRepository
{
    protected ActiveIngredientRepository(AppDbContext context) : base(context)
    {
    }
}
