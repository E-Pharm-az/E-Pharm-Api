using PharmaPortalService.Infrastructure.Context.Entities.Base;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

namespace PharmaPortalService.Infrastructure.Context.Entities.Junctions;

public class ProductRouteOfAdministration : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int RouteOfAdministrationId { get; set; }
    public RouteOfAdministration RouteOfAdministration { get; set; }
}
