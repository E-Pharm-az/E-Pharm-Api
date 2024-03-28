using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class SpecialRequirement : BaseEntity
{
    public int PharmaCompanyId { get; set; }
    public PharmaCompany PharmaCompany { get; set; }
    
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
