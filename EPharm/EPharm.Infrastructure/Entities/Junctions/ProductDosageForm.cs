using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Infrastructure.Entities.Junctions;

public class ProductDosageForm : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int DosageFormId { get; set; }
    public DosageForm DosageForm { get; set; }
}
