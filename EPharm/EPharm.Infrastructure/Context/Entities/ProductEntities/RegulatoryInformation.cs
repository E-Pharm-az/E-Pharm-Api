using EPharm.Infrastructure.Context.Entities.Base;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class RegulatoryInformation : BaseEntity
{
    public string RegulatoryStandards { get; set; }
    public DateTime ApprovalDate { get; set; }
    public string Certification { get; set; }

    public Product Product { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
