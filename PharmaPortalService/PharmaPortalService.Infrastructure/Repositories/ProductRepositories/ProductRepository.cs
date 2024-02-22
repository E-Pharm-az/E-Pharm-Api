using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;
using PharmaPortalService.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.ProductRepositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    protected ProductRepository(AppDbContext context) : base(context)
    {
    }
}
