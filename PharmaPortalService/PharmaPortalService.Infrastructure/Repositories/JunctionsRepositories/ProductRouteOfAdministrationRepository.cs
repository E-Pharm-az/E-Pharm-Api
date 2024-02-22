using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;
using PharmaPortalService.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.JunctionsRepositories;

public class ProductRouteOfAdministrationRepository : Repository<ProductRouteOfAdministration>, IProductRouteOfAdministrationRepository
{
    protected ProductRouteOfAdministrationRepository(AppDbContext context) : base(context)
    {
    }
}
