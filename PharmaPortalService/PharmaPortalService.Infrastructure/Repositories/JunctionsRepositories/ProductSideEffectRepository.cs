using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;
using PharmaPortalService.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.JunctionsRepositories;

public class ProductSideEffectRepository : Repository<ProductSideEffect>, IProductSideEffectRepository
{
    protected ProductSideEffectRepository(AppDbContext context) : base(context)
    {
    }
}
