using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Junctions;
using EPharm.Infrastructure.Repositories.Base;

namespace EPharm.Infrastructure.Repositories.Junctions;

public class WarehouseProductRepository(AppDbContext context) : Repository<WarehouseProduct>(context), IWarehouseProductRepository
{
    public async Task InsertWarehouseProductAsync(WarehouseProduct warehouseProduct)
    {
        await Entities.AddAsync(warehouseProduct);
        await base.SaveChangesAsync();
    }
}
