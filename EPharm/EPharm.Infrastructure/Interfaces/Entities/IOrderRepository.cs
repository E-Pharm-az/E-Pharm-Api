using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Entities;

public interface IOrderRepository : IRepository<Order>
{
    public Task<IEnumerable<Order>> GetAllOrdersAsync();
    public Task<Order?> GetOrderByIdAsync(int orderId);
    public Task<IEnumerable<Order>> GetAllUserOrdersAsync(string userId);
    public Task<Order?> GetOrderByTrackingNumberAsync(string trackingNumber);
}
