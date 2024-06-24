using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.Junctions;

namespace EPharm.Infrastructure.Entities.ProductEntities;

public class Allergy : BaseEntity
{
    public string Name { get; set; }
    
    public ICollection<ProductAllergy> Products { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
