using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.Junctions;

namespace EPharm.Infrastructure.Entities.ProductEntities;

public class DosageForm : BaseEntity
{
    public string Name { get; set; }
    
    public ICollection<ProductDosageForm> Products { get; set; }
}
