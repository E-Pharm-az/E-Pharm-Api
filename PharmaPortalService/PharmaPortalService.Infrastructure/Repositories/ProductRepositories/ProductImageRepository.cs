using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;
using PharmaPortalService.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.ProductRepositories;

public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
{
    protected ProductImageRepository(AppDbContext context) : base(context)
    {
    }
}
