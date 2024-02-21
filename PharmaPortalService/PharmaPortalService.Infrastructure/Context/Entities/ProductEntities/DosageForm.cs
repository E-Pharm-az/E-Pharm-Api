using PharmaPortalService.Infrastructure.Context.Entities.Base;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;

namespace PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

public class DosageForm : BaseEntity
{
    public string DosageFormName { get; set; }
    
    public ICollection<ProductDosageForm> Products { get; set; }
}
