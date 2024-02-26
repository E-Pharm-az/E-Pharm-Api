using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Infrastructure.Context.Entities.Junctions;

public class ProductDosageForm : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int DosageFormId { get; set; }
    public DosageForm DosageForm { get; set; }
}
