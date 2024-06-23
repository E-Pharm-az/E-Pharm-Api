using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class RegulatoryInformation : BaseEntity
{
    public int PharmaCompanyId { get; set; }
    public Pharmacy Pharmacy { get; set; }

    public string Name { get; set; }
    public DateTime ApprovalDate { get; set; }
    public string Certification { get; set; }

    public ICollection<Product> Product { get; set; }

    public DateTime CreatedAt { get; set; }
}
