using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Entities;

public interface IOrderRepository : IRepository<Order>
{
    public Task<List<IEnumerable<OrderProduct>>> GetAllPharmacyOrdersAsync(int pharmacyId);
    public Task<IEnumerable<Order>> GetAllUserOrdersAsync(string userId);
    public Task<Order?> GetOrderByTrackingNumberAsync(string trackingNumber);
}
