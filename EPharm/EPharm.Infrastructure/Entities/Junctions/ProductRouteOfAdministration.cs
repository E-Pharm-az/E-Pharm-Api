using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Infrastructure.Entities.Junctions;

public class ProductRouteOfAdministration : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int RouteOfAdministrationId { get; set; }
    public RouteOfAdministration RouteOfAdministration { get; set; }
}
