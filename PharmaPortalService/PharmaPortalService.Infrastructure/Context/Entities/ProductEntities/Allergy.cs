using PharmaPortalService.Infrastructure.Context.Entities.Base;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;

namespace PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

public class Allergy : BaseEntity
{
    public string Description { get; set; }
    
    public ICollection<ProductAllergy> Products { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
