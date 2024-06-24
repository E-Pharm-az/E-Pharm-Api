using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Entities.ProductEntities;

public class RegulatoryInformation : BaseEntity
{
    public int PharmacyId { get; set; }
    public Pharmacy Pharmacy { get; set; }

    public string Name { get; set; }
    public DateTime ApprovalDate { get; set; }
    public string Certification { get; set; }

    public ICollection<Product> Product { get; set; }

    public DateTime CreatedAt { get; set; }
}
