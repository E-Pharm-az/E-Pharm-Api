using EPharm.Infrastructure.Entities.Base;

namespace EPharm.Infrastructure.Entities.PharmaEntities;

public class PharmacyStaff : BaseEntity
{
    public string ExternalId { get; set; }
    public string Email { get; set; }
    public int? PharmacyId { get; set; }
    public Pharmacy? Pharmacy { get; set; }
    
    public DateTime CreatedAt { get; set; } 
}
