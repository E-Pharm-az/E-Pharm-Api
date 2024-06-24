using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Infrastructure.Entities.Junctions;

public class ProductSideEffect : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int SideEffectId { get; set; }
    public SideEffect SideEffect { get; set; }
}
