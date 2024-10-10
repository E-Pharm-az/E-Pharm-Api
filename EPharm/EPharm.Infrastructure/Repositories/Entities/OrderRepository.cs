using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Models;
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

    public async Task<PageResult<Order>> GetAllPharmacyOrdersAsync(int pharmacyId, int page, int limit)
    {
        return await base.GetPageAsync(page, limit, new QueryParameters<Order>
        {
            Filter = o => o.OrderProducts.Any(op => op.PharmacyId == pharmacyId),
            Include = query => query.Include(o => o.OrderProducts.Where(op => op.PharmacyId == pharmacyId))
        });
    }
    public async Task<IEnumerable<Order>> GetAllUserOrdersAsync(string userId) =>
        await Entities.Where(o => o.UserId == userId && o.IsPaid == true)
            .Include(o => o.OrderProducts)
            .ThenInclude(o => o.Product)
            .ToListAsync();

    public async Task<IEnumerable<Order>> GetAllOrdersByDate(DateTime startDate, DateTime endDate, Func<IQueryable<Order>, IQueryable<Order>>? additionalQuery = null)
    {
        var baseQuery = Entities.Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate);

        var finalQuery = additionalQuery != null ? additionalQuery(baseQuery) : baseQuery;

        return await finalQuery
            .Include(o => o.OrderProducts)
            .ThenInclude(o => o.Product)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByTrackingNumberAsync(string trackingNumber) =>
        await Entities.Include(o => o.OrderProducts)
            .ThenInclude(o => o.Product)
            .FirstOrDefaultAsync(o => o.TrackingId == trackingNumber);
}
