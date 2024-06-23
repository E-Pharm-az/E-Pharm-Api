using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.Junctions;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class Order : BaseEntity
{
    public string? UserId { get; set; }
    public string TrackingId { get; set; }
    public string Status { get; set; }
    public int TotalPrice { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FullName { get; set; }
    public string ShippingAddress { get; set; }
    public ICollection<OrderProduct> OrderProducts;
    
    public DateTime CreatedAt { get; set; }
}
