using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Infrastructure.Entities.Junctions;

public class IndicationProduct : BaseEntity
{
    public int IndicationId { get; set; }
    public Indication Indication { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; }
}
