using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;
using PharmaPortalService.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.JunctionsRepositories;

public class ProductActiveIngredientRepository : Repository<ProductActiveIngredient>, IProductActiveIngredientRepository
{
    protected ProductActiveIngredientRepository(AppDbContext context) : base(context)
    {
    }
}
