using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

namespace EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

public interface IOrderRepository : IRepository<Order>
{
    public Task<IEnumerable<Order>> GetAllOrdersAsync();
    public Task<Order?> GetOrderByIdAsync(int orderId);
    public Task<IEnumerable<Order>> GetAllUserOrdersAsync(string userId);
    public Task<Order?> GetOrderByTrackingNumberAsync(string trackingNumber);
}
