using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

namespace EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

public interface IOrderRepository : IRepository<Order>
{
    public Task<IEnumerable<Order>> GetAllCompanyOrdersAsync(int companyId);
    public Task<IEnumerable<Order>> GetAllWarehouseOrdersAsync(int warehouseId);
    public Task<IEnumerable<Order>> GetAllUserOrdersAsync(string userId);
    public Task<Order?> GetOrderByTrackingNumberAsync(string trackingNumber);
}
