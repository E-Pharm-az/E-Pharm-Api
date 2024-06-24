using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.Junctions;

namespace EPharm.Infrastructure.Entities.ProductEntities;

public class Indication : BaseEntity
{ 
    public string Name { get; set; }
    public string Description { get; set; }
    
    public ICollection<IndicationProduct> Products { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
