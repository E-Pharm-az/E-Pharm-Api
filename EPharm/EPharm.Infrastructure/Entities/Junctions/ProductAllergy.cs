using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Infrastructure.Entities.Junctions;

public class ProductAllergy : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int AllergyId { get; set; }
    public Allergy Allergy { get; set; }
}
