using EPharm.Domain.Dtos.ActiveIngredientDto;
using EPharm.Domain.Dtos.AllergyDto;
using EPharm.Domain.Dtos.DosageFormDto;
using EPharm.Domain.Dtos.IndicationDto;
using EPharm.Domain.Dtos.ManufacturerDto;
using EPharm.Domain.Dtos.PharmacyDtos;
using EPharm.Domain.Dtos.RegulatoryInformationDto;
using EPharm.Domain.Dtos.RouteOfAdministrationDto;
using EPharm.Domain.Dtos.SideEffectDto;
using EPharm.Domain.Dtos.SpecialRequirementsDto;
using EPharm.Domain.Dtos.UsageWarningDto;

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
    public ICollection<GetDosageFormDto> DosageForms { get; set; }
    public ICollection<GetRouteOfAdministrationDto> RouteOfAdministrations { get; set; }
    public ICollection<GetSideEffectDto> SideEffects { get; set; }
    public ICollection<GetUsageWarningDto> UsageWarnings { get; set; }
    public ICollection<GetAllergyDto> Allergies { get; set; }
    public ICollection<GetIndicationDto> Indications { get; set; }

    public string BatchNumber { get; set; }
    public string Barcode { get; set; }
    public decimal PackagingWeight { get; set; }
}