using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.CommonEntities;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Infrastructure.Context.Entities.PharmaEntities;

public class PharmaCompany : BaseEntity
{
    public string CompanyName { get; set; }
    public string Location { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public int AddressId { get; set; }
    public Address Address { get; set; }
    public string PharmaCompanyOwnerId { get; set; } 
    
    public ICollection<PharmaCompanyManager> PharmaCompanyManagers { get; set; }
    public ICollection<Product> Products { get; set; }
    public ICollection<ActiveIngredient> ActiveIngredients { get; set; }
    public ICollection<Allergy> Allergies { get; set; }
    public ICollection<DosageForm> DosageForms { get; set; }
    public ICollection<Indication> Indications { get; set; }
    public ICollection<Manufacturer> Manufacturers { get; set; }
    public ICollection<RegulatoryInformation> RegulatoryInformations { get; set; }
    public ICollection<RouteOfAdministration> RouteOfAdministrations { get; set; }
    public ICollection<SideEffect> SideEffects { get; set; }
    public ICollection<SpecialRequirement> SpecialRequirements { get; set; }
    public ICollection<UsageWarning> UsageWarnings { get; set; }
    
    public DateTime CreatedAt { get; set; } 
}
