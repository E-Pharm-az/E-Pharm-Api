using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class ProductRouteOfAdministrationRepository : Repository<ProductRouteOfAdministration>, IProductRouteOfAdministrationRepository
{
    protected ProductRouteOfAdministrationRepository(AppDbContext context) : base(context)
    {
    }
}
