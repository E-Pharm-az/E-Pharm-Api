using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class Manufacturer : BaseEntity
{
    public int PharmaCompanyId { get; set; }
    public PharmaCompany PharmaCompany { get; set; }
    
    public string ManufacturerName { get; set; }
    public string ManufacturerLocation { get; set; }
    
    public ICollection<Product> Products { get; set; }
    public DateTime CreatedAt { get; set; }
}
