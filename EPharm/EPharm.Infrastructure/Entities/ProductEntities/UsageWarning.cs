using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.Junctions;

namespace EPharm.Infrastructure.Entities.ProductEntities;

public class UsageWarning : BaseEntity
{
    public string Name { get; set; }
    
    public ICollection<ProductUsageWarning> Products { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
