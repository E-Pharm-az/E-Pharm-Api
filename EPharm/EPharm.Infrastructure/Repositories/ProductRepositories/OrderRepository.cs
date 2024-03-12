using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class OrderRepository(AppDbContext context) : Repository<Order>(context), IOrderRepository
{
    public async Task<IEnumerable<Order>> GetAllOrdersAsync() =>
        await Entities
            .Include(o => o.OrderProducts)
            .ThenInclude(o => o.Product)
            .ToListAsync();

    public async Task<Order?> GetOrderByIdAsync(int orderId) =>
        await Entities
            .Include(o => o.OrderProducts)
            .ThenInclude(o => o.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId);

    public async Task<IEnumerable<Order>> GetAllUserOrdersAsync(string userId) =>
        await Entities.Where(o => o.UserId == userId)
            .Include(o => o.OrderProducts)
            .ThenInclude(o => o.Product)
            .ToListAsync();

    public async Task<Order?> GetOrderByTrackingNumberAsync(string trackingNumber) =>
        await Entities.Include(o => o.OrderProducts)
            .ThenInclude(o => o.Product)
            .FirstOrDefaultAsync(o => o.TrackingId == trackingNumber);
}
