using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.Junctions;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class UsageWarning : BaseEntity
{
    public string Name { get; set; }
    
    public ICollection<ProductUsageWarning> Products { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
