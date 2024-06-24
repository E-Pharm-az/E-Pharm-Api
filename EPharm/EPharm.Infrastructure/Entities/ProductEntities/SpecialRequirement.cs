using EPharm.Infrastructure.Entities.Base;
using EPharm.Infrastructure.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Entities.ProductEntities;

public class SpecialRequirement : BaseEntity
{
    public int PharmaCompanyId { get; set; }
    public Pharmacy Pharmacy { get; set; }
    
    public string Name { get; set; } 
    public int MinimumAgeInMonthsRequirement { get; set; }
    public int MaximumAgeInMonthsRequirement { get; set; }
    public decimal MinimumWeighInKgRequirement { get; set; }
    public decimal MaximumWeighInKgRequirement { get; set; }
    public string MedicalConditionsDescription { get; set; }
    public string OtherRequirementsDescription { get; set; } 
    
    public ICollection<Product> Products { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
