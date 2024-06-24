using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Junctions;

public interface IWarehouseProductRepository : IRepository<WarehouseProduct>
{
    public Task InsertWarehouseProductAsync(WarehouseProduct warehouseProduct);
}
