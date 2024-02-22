using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;
using PharmaPortalService.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.ProductRepositories;

public class RouteOfAdministrationRepository : Repository<RouteOfAdministration>, IRouteOfAdministrationRepository
{
    protected RouteOfAdministrationRepository(AppDbContext context) : base(context)
    {
    }
}
