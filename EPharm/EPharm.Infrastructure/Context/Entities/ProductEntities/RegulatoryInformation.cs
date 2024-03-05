using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class RegulatoryInformation : BaseEntity
{
    public int PharmaCompanyId { get; set; }
    public PharmaCompany PharmaCompany { get; set; }

    public string RegulatoryStandards { get; set; }
    public DateTime ApprovalDate { get; set; }
    public string Certification { get; set; }

    public Product Product { get; set; }

    public DateTime CreatedAt { get; set; }
}
