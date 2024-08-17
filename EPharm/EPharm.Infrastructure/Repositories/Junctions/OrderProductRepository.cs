using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Junctions;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Repositories.Base;

namespace EPharm.Infrastructure.Repositories.Junctions;

public class OrderProductRepository(AppDbContext context) : Repository<OrderProduct>(context), IOrderProductRepository
{
    public async Task CreateManyOrderProductAsync(IEnumerable<OrderProduct> orderProducts)
    {
        await Entities.AddRangeAsync(orderProducts);
        await base.SaveChangesAsync();
    }
}
