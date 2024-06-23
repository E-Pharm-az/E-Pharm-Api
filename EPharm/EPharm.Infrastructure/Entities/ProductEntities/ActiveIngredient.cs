using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class ActiveIngredient : BaseEntity
{
    public int PharmaCompanyId { get; set; }
    public Pharmacy Pharmacy { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<ProductActiveIngredient> Products { get; set; }
    public DateTime CreatedAt { get; set; }
}
