using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Junctions;

public interface IProductRouteOfAdministrationRepository : IRepository<ProductRouteOfAdministration>
{
    public Task InsertAsync(int productId, int[] routeOfAdministrationsIds);
}
