using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.Junctions;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class DosageForm : BaseEntity
{
    public string DosageFormName { get; set; }
    
    public ICollection<ProductDosageForm> Products { get; set; }
}