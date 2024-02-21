using PharmaPortalService.Infrastructure.Context.Entities.Base;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;

namespace PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

public class Indication : BaseEntity
{ 
    public string IndicationsName { get; set; }
    public string IndicationsDescription { get; set; }
    
    public ICollection<IndicationProduct> IndicationProducts { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
