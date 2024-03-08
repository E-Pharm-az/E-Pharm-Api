using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.ProductEntities;

namespace EPharm.Infrastructure.Context.Entities.PharmaEntities;

public class PharmaCompany : BaseEntity
{
    public string PharmaCompanyOwnerId { get; set; } 
    public string TIN { get; set; }
    public string CompanyName { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public string StreetAddress { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Region { get; set; }
    public int BuildingNumber { get; set; }
    public int Floor { get; set; }
    public int RoomNumber { get; set; }  
    
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
