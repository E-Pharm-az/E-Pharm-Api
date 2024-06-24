using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Infrastructure.Entities.Junctions;

public class WarehouseProduct : BaseEntity
{
    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int Quantity { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
