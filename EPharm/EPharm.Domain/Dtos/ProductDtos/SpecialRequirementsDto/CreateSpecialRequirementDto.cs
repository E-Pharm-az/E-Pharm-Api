using System.ComponentModel.DataAnnotations;

namespace EPharm.Domain.Dtos.ProductDtos.SpecialRequirementsDto;

public class CreateSpecialRequirementDto
{
    public int MinimumAgeInMonthsRequirement { get; set; }
    public int MaximumAgeInMonthsRequirement { get; set; }
    public decimal MinimumWeighInKgRequirement { get; set; }
    public decimal MaximumWeighInKgRequirement { get; set; }
    public string MedicalConditionsDescription { get; set; }
    public string OtherRequirementsDescription { get; set; } 
}
