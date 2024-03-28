using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.Junctions;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class Indication : BaseEntity
{ 
    public string Name { get; set; }
    public string Description { get; set; }
    
    public ICollection<IndicationProduct> Products { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
