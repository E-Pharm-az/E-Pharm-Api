using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class RouteOfAdministrationRepository : Repository<RouteOfAdministration>, IRouteOfAdministrationRepository
{
    protected RouteOfAdministrationRepository(AppDbContext context) : base(context)
    {
    }
}
