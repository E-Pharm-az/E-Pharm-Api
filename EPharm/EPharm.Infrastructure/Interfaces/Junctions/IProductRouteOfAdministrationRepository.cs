using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Junctions;

public interface IProductRouteOfAdministrationRepository : IRepository<ProductRouteOfAdministration>
{
    public Task InsertProductRouteOfAdministrationAsync(int productId, int[] routeOfAdministrationsIds);
}
