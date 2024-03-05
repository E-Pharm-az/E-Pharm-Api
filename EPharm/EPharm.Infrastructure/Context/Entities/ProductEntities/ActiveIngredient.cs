using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class ActiveIngredient : BaseEntity
{
    public int PharmaCompanyId { get; set; }
    public PharmaCompany PharmaCompany { get; set; }
    
    public string IngredientName { get; set; }
    public string IngredientDescription { get; set; }

    public ICollection<ProductActiveIngredient> Products { get; set; }
    public DateTime CreatedAt { get; set; }
}
