using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.Junctions;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class RouteOfAdministration : BaseEntity
{
    public string Description { get; set; }
    
    public ICollection<ProductRouteOfAdministration> Products { get; set; }
    public DateTime CreatedAt { get; set; }
}
