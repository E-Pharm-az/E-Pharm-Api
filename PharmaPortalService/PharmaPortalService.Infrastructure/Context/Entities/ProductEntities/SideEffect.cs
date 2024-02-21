using PharmaPortalService.Infrastructure.Context.Entities.Base;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;

namespace PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

public class SideEffect : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public ICollection<ProductSideEffect> Products { get; set; }
    public DateTime CreatedAt { get; set; }
}
