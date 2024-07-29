namespace EPharm.Domain.Dtos.SpecialRequirementsDto;

public class GetSpecialRequirementDto
{
    public int Id { get; set; }
    public int PharmacyId { get; set; }
    public string Name { get; set; } 
    public int MinimumAgeInMonthsRequirement { get; set; }
    public int MaximumAgeInMonthsRequirement { get; set; }
    public decimal MinimumWeighInKgRequirement { get; set; }
    public decimal MaximumWeighInKgRequirement { get; set; }
    public string MedicalConditionsDescription { get; set; }
    public string OtherRequirementsDescription { get; set; }
}
