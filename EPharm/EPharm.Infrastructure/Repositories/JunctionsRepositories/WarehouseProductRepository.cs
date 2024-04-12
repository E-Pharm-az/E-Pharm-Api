using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class WarehouseProductRepository(AppDbContext context) : Repository<WarehouseProduct>(context), IWarehouseProductRepository
{
    public async Task InsertWarehouseProductAsync(WarehouseProduct warehouseProduct)
    {
        await Entities.AddAsync(warehouseProduct);
        await base.SaveChangesAsync();
    }
}
