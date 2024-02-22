using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;
using PharmaPortalService.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.ProductRepositories;

public class ManufacturerRepository : Repository<Manufacturer>, IManufacturerRepository
{
    protected ManufacturerRepository(AppDbContext context) : base(context)
    {
    }
}
