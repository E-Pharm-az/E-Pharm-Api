using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Junctions;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Repositories.Base;

namespace EPharm.Infrastructure.Repositories.Junctions;

public class OrderProductRepository(AppDbContext context, IProductRepository productRepository)
    : Repository<OrderProduct>(context), IOrderProductRepository
{
    public async Task CreateManyOrderProductAsync(IEnumerable<OrderProduct> orderProducts)
    {
        await context.OrderProducts.AddRangeAsync(orderProducts);
        await context.SaveChangesAsync();
    }
}
