using PharmaPortalService.Infrastructure.Context.Entities.Base;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

namespace PharmaPortalService.Infrastructure.Context.Entities.Junctions;

public class ProductAllergy : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int AllergyId { get; set; }
    public Allergy Allergy { get; set; }
}
