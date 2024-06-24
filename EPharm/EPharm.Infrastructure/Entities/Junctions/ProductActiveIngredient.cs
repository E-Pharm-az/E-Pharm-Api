using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Infrastructure.Entities.Junctions;

public class ProductActiveIngredient : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int ActiveIngredientId { get; set; }
    public ActiveIngredient ActiveIngredient { get; set; }

    public decimal Quantity { get; set; }
}
