using PharmaPortalService.Infrastructure.Context.Entities.Base;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

namespace PharmaPortalService.Infrastructure.Context.Entities.Junctions;

public class ProductDosageForm : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int DosageFormId { get; set; }
    public DosageForm DosageForm { get; set; }
}
