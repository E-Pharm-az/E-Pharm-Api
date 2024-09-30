using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Entities;

public interface IOrderRepository : IRepository<Order>
{
    public Task<IEnumerable<Order>> GetAllPharmacyOrdersAsync(int pharmacyId);
    public Task<IEnumerable<Order>> GetAllUserOrdersAsync(string userId);
    public Task<IEnumerable<Order>> GetAllOrdersByDate(DateTime startDate, DateTime endDate, Func<IQueryable<Order>, IQueryable<Order>>? additionalQuery = null);
    public Task<Order?> GetOrderByTrackingNumberAsync(string trackingNumber);
}
