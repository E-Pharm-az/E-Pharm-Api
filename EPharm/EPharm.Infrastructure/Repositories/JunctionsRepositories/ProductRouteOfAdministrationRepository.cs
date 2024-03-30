using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class ProductRouteOfAdministrationRepository(AppDbContext context, IRouteOfAdministrationRepository routeOfAdministrationRepository)
    : Repository<ProductRouteOfAdministration>(context), IProductRouteOfAdministrationRepository
{
    public async Task InsertProductRouteOfAdministrationAsync(int productId, int[] routeOfAdministrationsIds)
    {
        foreach (var routeOfAdministrationsId in routeOfAdministrationsIds)
        {
            var routeOfAdministration = await routeOfAdministrationRepository.GetByIdAsync(routeOfAdministrationsId);
            
            if (routeOfAdministration is null)
                throw new ArgumentException("Route of administration not found");
            
            await Entities.AddAsync(
                new ProductRouteOfAdministration
                {
                    ProductId = productId,
                    RouteOfAdministrationId = routeOfAdministrationsId
                }
            );
        }
        
        await base.SaveChangesAsync();
    }
}
