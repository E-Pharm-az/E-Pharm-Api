using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.PharmaEntities;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Infrastructure.Entities.Junctions;

public class OrderProduct : BaseEntity
{
    public int? PharmacyId { get; set; }
    public Pharmacy? Pharmacy { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int? WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
    public int Quantity { get; set; }
    public int? Frequency { get; set; }
    public int? SupplyDuration { get; set; }
    public double TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}
