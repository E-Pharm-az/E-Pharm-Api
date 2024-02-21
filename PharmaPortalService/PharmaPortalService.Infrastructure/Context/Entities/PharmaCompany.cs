using PharmaPortalService.Infrastructure.Context.Entities.Base;

namespace PharmaPortalService.Infrastructure.Context.Entities;

public class PharmaCompany : BaseEntity
{
    public string CompanyName { get; set; }
    public string Location { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public ICollection<PharmaCompanyManager> PharmaCompanyManagers { get; set; }
    public DateTime CreatedAt { get; set; } 
}
