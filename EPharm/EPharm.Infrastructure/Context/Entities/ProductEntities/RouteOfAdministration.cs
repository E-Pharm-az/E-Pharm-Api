using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class RouteOfAdministration : BaseEntity
{
    public int PharmaCompanyId { get; set; }
    public PharmaCompany PharmaCompany { get; set; }

    public string Description { get; set; }
    
    public ICollection<ProductRouteOfAdministration> Products { get; set; }
    public DateTime CreatedAt { get; set; }
}
