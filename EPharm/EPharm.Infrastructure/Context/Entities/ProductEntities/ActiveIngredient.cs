using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.Junctions;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class ActiveIngredient : BaseEntity
{
    public string IngredientName { get; set; }
    public string IngredientDescription { get; set; }

    public ICollection<ProductActiveIngredient> Products { get; set; }
    public DateTime CreatedAt { get; set; }
}
