using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Entities.ProductEntities;

public class Manufacturer : BaseEntity
{
    public int PharmacyId { get; set; }
    public Pharmacy Pharmacy { get; set; }
    
    public string Name { get; set; }
    public string Country { get; set; }
    public string Website { get; set; }
    public string Email { get; set; }
    
    public ICollection<Product> Products { get; set; }
    public DateTime CreatedAt { get; set; }
}
