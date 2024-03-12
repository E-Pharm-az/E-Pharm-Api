using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

namespace EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

public interface IOrderProductRepository : IRepository<OrderProduct>
{
    public Task<int> InsertOrderProductAsync(int orderId, int productsId, int quantity);
}
