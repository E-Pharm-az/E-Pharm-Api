using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class SideEffect : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public ICollection<ProductSideEffect> Products { get; set; }
    public DateTime CreatedAt { get; set; }
}
