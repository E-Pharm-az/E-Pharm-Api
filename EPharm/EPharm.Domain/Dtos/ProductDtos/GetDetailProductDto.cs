using EPharm.Domain.Dtos.ActiveIngredientDto;
using EPharm.Domain.Dtos.AttributeDtos;
using EPharm.Domain.Dtos.ManufacturerDto;
using EPharm.Domain.Dtos.PharmacyDtos;
using EPharm.Domain.Dtos.RegulatoryInformationDto;
using EPharm.Domain.Dtos.SpecialRequirementsDto;

namespace EPharm.Domain.Dtos.ProductDtos;

public class GetDetailProductDto : GetProductDto
{
    public GetPharmacyDto Pharmacy { get; set; }

    public string? ApprovedByAdminId { get; set; }
    public int MaxDayFrequency { get; set; }
    public int MaxSupplyInDaysDays { get; set; }
    public string ContraindicationsDescription { get; set; }
    public string StorageConditionDescription { get; set; }

    public GetSpecialRequirementDto SpecialRequirement { get; set; }
    public GetManufacturerDto Manufacturer { get; set; }
    public GetRegulatoryInformationDto RegulatoryInformation { get; set; }

    public ICollection<GetProductActiveIngredientDto> ActiveIngredients { get; set; }
    public ICollection<GetAttributeDto> DosageForms { get; set; }
    public ICollection<GetAttributeDto> RouteOfAdministrations { get; set; }
    public ICollection<GetAttributeDto> SideEffects { get; set; }
    public ICollection<GetAttributeDto> UsageWarnings { get; set; }
    public ICollection<GetAttributeDto> Allergies { get; set; }
    public ICollection<GetAttributeDto> Indications { get; set; }

    public string BatchNumber { get; set; }
    public string Barcode { get; set; }
    public decimal PackagingWeight { get; set; }
}
