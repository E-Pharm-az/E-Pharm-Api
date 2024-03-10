using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class OrderRepository(AppDbContext context) : Repository<Order>(context), IOrderRepository
{
    public async Task<IEnumerable<Order>> GetAllCompanyOrdersAsync(int companyId) =>
        await Entities.Where(o => o.PharmaCompanyId == companyId).ToListAsync();

    public async Task<IEnumerable<Order>> GetAllWarehouseOrdersAsync(int warehouseId) =>
        await Entities.Where(o => o.WarehouseId == warehouseId).ToListAsync();

    public async Task<IEnumerable<Order>> GetAllUserOrdersAsync(string userId) =>
        await Entities.Where(o => o.UserId == userId).ToListAsync();

    public async Task<Order?> GetOrderByTrackingNumberAsync(string trackingNumber) =>
        await Entities.FirstOrDefaultAsync(o => o.TrackingId == trackingNumber);
}
