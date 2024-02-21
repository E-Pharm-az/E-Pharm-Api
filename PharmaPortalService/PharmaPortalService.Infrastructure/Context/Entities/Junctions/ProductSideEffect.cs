using PharmaPortalService.Infrastructure.Context.Entities.Base;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

namespace PharmaPortalService.Infrastructure.Context.Entities.Junctions;

public class ProductSideEffect : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int SideEffectId { get; set; }
    public SideEffect SideEffect { get; set; }
}
