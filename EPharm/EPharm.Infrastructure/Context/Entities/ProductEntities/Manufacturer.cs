using EPharm.Infrastructure.Context.Entities.Base;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class Manufacturer : BaseEntity
{
    public string ManufacturerName { get; set; }
    public string ManufacturerLocation { get; set; }
    
    public ICollection<Product> Products { get; set; }
    public DateTime CreatedAt { get; set; }
}
