using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;
using PharmaPortalService.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.ProductRepositories;

public class SideEffectRepository : Repository<SideEffect>, ISideEffectRepository
{
    protected SideEffectRepository(AppDbContext context) : base(context)
    {
    }
}
