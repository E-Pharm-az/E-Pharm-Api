using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Junctions;
using EPharm.Infrastructure.Repositories.Base;

namespace EPharm.Infrastructure.Repositories.Junctions;

public class WarehouseProductRepository(AppDbContext context) : Repository<WarehouseProduct>(context), IWarehouseProductRepository
{
    public new async Task InsertAsync(WarehouseProduct warehouseProduct)
    {
        await Entities.AddAsync(warehouseProduct);
        await base.SaveChangesAsync();
    }
}
