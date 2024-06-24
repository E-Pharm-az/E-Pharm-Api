using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Junctions;

public interface IOrderProductRepository : IRepository<OrderProduct>
{
    public Task CreateManyOrderProductAsync(IEnumerable<OrderProduct> orderProducts);
}
