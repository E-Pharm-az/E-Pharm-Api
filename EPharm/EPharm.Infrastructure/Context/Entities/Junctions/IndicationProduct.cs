using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Infrastructure.Context.Entities.Junctions;

public class IndicationProduct : BaseEntity
{
    public int IndicationId { get; set; }
    public Indication Indication { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; }
}
