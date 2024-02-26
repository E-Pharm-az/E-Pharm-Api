using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.Junctions;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class Indication : BaseEntity
{ 
    public string IndicationsName { get; set; }
    public string IndicationsDescription { get; set; }
    
    public ICollection<IndicationProduct> Products { get; set; }
    
    public DateTime CreatedAt { get; set; }
}