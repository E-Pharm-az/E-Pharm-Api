using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.Entities;

public class OrderRepository(AppDbContext context) : Repository<Order>(context), IOrderRepository
{
    public override async Task<Order?> GetByIdAsync(int id) =>
        await Entities
            .Include(o => o.OrderProducts)
            .ThenInclude(o => o.Product)            
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IEnumerable<Order>> GetAllPharmacyOrdersAsync(int pharmacyId) =>
        await Entities
            .Include(o => o.OrderProducts.Where(op => op.PharmacyId == pharmacyId))
            .ThenInclude(o => o.Product)
            .AsNoTracking()
            .ToListAsync();

    public async Task<IEnumerable<Order>> GetAllUserOrdersAsync(string userId) =>
        await Entities.Where(o => o.UserId == userId && o.IsPaid == true)
            .Include(o => o.OrderProducts)
            .ThenInclude(o => o.Product)
            .ToListAsync();

    public async Task<Order?> GetOrderByTrackingNumberAsync(string trackingNumber) =>
        await Entities.Include(o => o.OrderProducts)
            .ThenInclude(o => o.Product)
            .FirstOrDefaultAsync(o => o.TrackingId == trackingNumber);
}
