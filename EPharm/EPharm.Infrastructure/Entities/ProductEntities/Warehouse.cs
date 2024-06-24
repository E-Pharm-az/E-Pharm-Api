using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Entities.ProductEntities;

public class Warehouse : BaseEntity
{
    public string Name { get; set; }
    public string Address { get; set; }
    public int PharmaCompanyId { get; set; }
    public Pharmacy Pharmacy { get; set; }
    public ICollection<WarehouseProduct> WarehouseProducts { get; set; }
    public ICollection<OrderProduct> OrderProducts;
    public DateTime CreatedAt { get; set; }
}
