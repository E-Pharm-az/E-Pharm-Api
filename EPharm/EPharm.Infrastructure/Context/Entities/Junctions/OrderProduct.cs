using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Infrastructure.Context.Entities.Junctions;

public class OrderProduct : BaseEntity
{
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public int? Frequency { get; set; }
    public int? SupplyDuration { get; set; }
    public double TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}
