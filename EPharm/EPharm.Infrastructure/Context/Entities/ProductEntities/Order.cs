using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.Junctions;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class Order : BaseEntity
{
    public string TrackingId { get; set; }
    public string OrderStatus { get; set; }
    public int TotalPrice { get; set; }
    public string ShippingAddress { get; set; }
    public string UserId { get; set; }
    public ICollection<OrderProduct> OrderProducts;
    
    public DateTime CreatedAt { get; set; }
}
