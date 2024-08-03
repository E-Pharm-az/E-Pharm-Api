using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Infrastructure.Entities.PharmaEntities;

public class Pharmacy : BaseEntity
{
    public string OwnerId { get; set; } 
    public string OwnerEmail { get; set; }
    public string? Name { get; set; }
    public string? TIN { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? Address { get; set; }
    public ICollection<PharmacyStaff> PharmacyStaff { get; set; }
    public ICollection<Warehouse> Warehouses { get; set; }
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
