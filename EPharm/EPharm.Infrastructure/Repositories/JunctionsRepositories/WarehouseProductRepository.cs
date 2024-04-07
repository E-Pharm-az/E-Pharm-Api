using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class WarehouseProductRepository(AppDbContext context) : Repository<WarehouseProduct>(context), IWarehouseProductRepository
{
    public async Task InsertWarehouseProductAsync(int productId, int warehouseId, int quantity)
    {
        await Entities.AddAsync(
            new WarehouseProduct
            {
                WarehouseId = warehouseId,
                ProductId = productId,
                Quantity = quantity
            }
        );

        await base.SaveChangesAsync();
    }
}
