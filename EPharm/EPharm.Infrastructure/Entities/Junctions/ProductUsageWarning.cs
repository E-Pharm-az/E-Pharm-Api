using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Infrastructure.Entities.Junctions;

public class ProductUsageWarning : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int UsageWarningId { get; set; }
    public UsageWarning UsageWarning { get; set; }
}
