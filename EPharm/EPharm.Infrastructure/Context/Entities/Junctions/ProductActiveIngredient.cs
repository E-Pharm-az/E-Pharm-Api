using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Infrastructure.Context.Entities.Junctions;

public class ProductActiveIngredient : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int ActiveIngredientId { get; set; }
    public ActiveIngredient ActiveIngredient { get; set; }

    public decimal Quantity { get; set; }
}
