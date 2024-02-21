using PharmaPortalService.Infrastructure.Context.Entities.Base;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

namespace PharmaPortalService.Infrastructure.Context.Entities.Junctions;

public class ProductActiveIngredient : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int ActiveIngredientId { get; set; }
    public ActiveIngredient ActiveIngredient { get; set; }

    public decimal Quantity { get; set; }
}
