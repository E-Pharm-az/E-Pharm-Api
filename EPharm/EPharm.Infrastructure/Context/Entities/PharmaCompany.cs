using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Infrastructure.Context.Entities;

public class PharmaCompany : BaseEntity
{
    public string CompanyName { get; set; }
    public string Location { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public int AddressId { get; set; }
    public Address Address { get; set; }
    public int PharmaCompanyOwnerId { get; set; } 
    public PharmaCompanyManager PharmaCompanyOwner { get; set; } 
    public ICollection<PharmaCompanyManager> PharmaCompanyManagers { get; set; }
    public ICollection<Product> Products { get; set; }
    public DateTime CreatedAt { get; set; } 
}
