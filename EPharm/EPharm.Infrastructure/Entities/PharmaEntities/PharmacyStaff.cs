using EPharm.Infrastructure.Context.Entities.Base;

namespace EPharm.Infrastructure.Context.Entities.PharmaEntities;

public class PharmacyStaff : BaseEntity
{
    public string ExternalId { get; set; }
    public string Email { get; set; }
    public int PharmaCompanyId { get; set; }
    public Pharmacy Pharmacy { get; set; }
    
    public DateTime CreatedAt { get; set; } 
}
