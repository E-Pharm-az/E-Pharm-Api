using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

namespace PharmaPortalService.Infrastructure.Context.Entities.Junctions;

public class ProductUsageWarning
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int UsageWarningId { get; set; }
    public UsageWarning UsageWarning { get; set; }
}
