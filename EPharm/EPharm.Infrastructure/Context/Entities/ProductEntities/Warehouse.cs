using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class Warehouse : BaseEntity
{
    public string Name { get; set; }
    public string Address { get; set; }
    public int PharmaCompanyId { get; set; }
    public PharmaCompany PharmaCompany { get; set; }
    public ICollection<WarehouseProduct> WarehouseProducts { get; set; }
    public ICollection<OrderProduct> OrderProducts;
    
    public DateTime CreatedAt { get; set; }
}
