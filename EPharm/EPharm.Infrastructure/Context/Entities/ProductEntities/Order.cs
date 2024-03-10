using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class Order : BaseEntity
{
    public string TrackingId { get; set; }
    public string OrderStatus { get; set; }
    public double TotalPrice { get; set; }
    public string ShippingAddress { get; set; }
    public string UserId { get; set; }
    public int PharmaCompanyId { get; set; }
    public PharmaCompany PharmaCompany { get; set; }
    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; }
    public ICollection<OrderProduct> OrderProducts;
    
    public DateTime CreatedAt { get; set; }
}
