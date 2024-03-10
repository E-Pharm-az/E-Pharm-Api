using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class Warehouse : BaseEntity
{
    public string Name { get; set; }
    public string Address { get; set; }
    public int PharmaCompanyId { get; set; }
    public PharmaCompany PharmaCompany { get; set; }
    public ICollection<Product> Products { get; set; }
    
    public ICollection<Order> Orders { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
