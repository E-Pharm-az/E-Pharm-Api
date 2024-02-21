using PharmaPortalService.Infrastructure.Context.Entities.Base;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;

namespace PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

public class RouteOfAdministration : BaseEntity
{
    public string Description { get; set; }
    
    public ICollection<ProductRouteOfAdministration> Products { get; set; }
    public DateTime CreatedAt { get; set; }
}
