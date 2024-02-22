using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;
using PharmaPortalService.Infrastructure.Interfaces;

namespace PharmaPortalService.Infrastructure.Repositories.ProductRepositories;

public class ActiveIngredientRepository : Repository<ActiveIngredient>, Interfaces.ProductRepositoriesInterfaces.IActiveIngredientRepository
{
    protected ActiveIngredientRepository(AppDbContext context) : base(context)
    {
    }
}
