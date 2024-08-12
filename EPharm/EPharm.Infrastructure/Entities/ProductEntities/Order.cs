using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.Junctions;

namespace EPharm.Infrastructure.Entities.ProductEntities;

public class Order : BaseEntity
{
    public string UserId { get; set; }
    public string TrackingId { get; set; }
    public string Status { get; set; }
    public int TotalPrice { get; set; }
    public string ShippingAddress { get; set; }
    public ICollection<OrderProduct> OrderProducts;
    
    public DateTime CreatedAt { get; set; }
}
