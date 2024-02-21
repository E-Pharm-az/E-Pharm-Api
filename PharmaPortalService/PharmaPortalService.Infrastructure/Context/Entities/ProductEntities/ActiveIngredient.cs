using PharmaPortalService.Infrastructure.Context.Entities.Base;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;

namespace PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

public class ActiveIngredient : BaseEntity
{
    public string IngredientName { get; set; }
    public string IngredientDescription { get; set; }

    public ICollection<ProductActiveIngredient> Products { get; set; }
    public DateTime CreatedAt { get; set; }
}
