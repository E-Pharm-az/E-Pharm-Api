using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

namespace EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

public interface IOrderProductRepository : IRepository<OrderProduct>
{
    public Task<Product> InsertOrderProductAsync(OrderProduct orderProduct);
}
