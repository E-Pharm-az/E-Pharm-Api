using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class UsageWarning : BaseEntity
{
    public int PharmaCompanyId { get; set; }
    public PharmaCompany PharmaCompany { get; set; }
    
    public string Description { get; set; }
    
    public ICollection<ProductUsageWarning> Products { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
