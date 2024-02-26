using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    protected ProductRepository(AppDbContext context) : base(context)
    {
    }
}
