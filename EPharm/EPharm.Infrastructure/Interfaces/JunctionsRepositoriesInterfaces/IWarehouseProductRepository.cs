using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

namespace EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

public interface IWarehouseProductRepository : IRepository<WarehouseProduct>
{
    public Task InsertWarehouseProductAsync(int warehouseId, int productId, int quantity);
}