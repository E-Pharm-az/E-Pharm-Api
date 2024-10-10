using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;
using EPharm.Infrastructure.Models;

namespace EPharm.Infrastructure.Interfaces.Entities;

public interface IOrderRepository : IRepository<Order>
{
    public Task<PageResult<Order>> GetAllPharmacyOrdersAsync(int pharmacyId, int page, int limit);
    public Task<IEnumerable<Order>> GetAllUserOrdersAsync(string userId);
    public Task<IEnumerable<Order>> GetAllOrdersByDate(DateTime startDate, DateTime endDate, Func<IQueryable<Order>, IQueryable<Order>>? additionalQuery = null);
    public Task<Order?> GetOrderByTrackingNumberAsync(string trackingNumber);
}
