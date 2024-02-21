using PharmaPortalService.Infrastructure.Context.Entities.Base;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;

namespace PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

public class UsageWarning : BaseEntity
{
    public string Description { get; set; }
    
    public ICollection<ProductUsageWarning> Products { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
