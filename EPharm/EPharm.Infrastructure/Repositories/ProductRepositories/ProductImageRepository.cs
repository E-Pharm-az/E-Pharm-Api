using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
{
    protected ProductImageRepository(AppDbContext context) : base(context)
    {
    }
}
