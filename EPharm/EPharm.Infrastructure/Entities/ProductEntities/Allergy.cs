using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.Junctions;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class Allergy : BaseEntity
{
    public string Name { get; set; }
    
    public ICollection<ProductAllergy> Products { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
