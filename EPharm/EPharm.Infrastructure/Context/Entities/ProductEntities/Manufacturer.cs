using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class Manufacturer : BaseEntity
{
    public int PharmaCompanyId { get; set; }
    public PharmaCompany PharmaCompany { get; set; }
    
    public string Name { get; set; }
    public string Country { get; set; }
    public string Website { get; set; }
    public string Email { get; set; }
    
    public ICollection<Product> Products { get; set; }
    public DateTime CreatedAt { get; set; }
}
