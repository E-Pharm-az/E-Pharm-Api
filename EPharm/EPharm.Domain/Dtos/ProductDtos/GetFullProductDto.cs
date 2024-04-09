using EPharm.Domain.Dtos.ActiveIngredientDto;
using EPharm.Domain.Dtos.AllergyDto;
using EPharm.Domain.Dtos.DosageFormDto;
using EPharm.Domain.Dtos.IndicationDto;
using EPharm.Domain.Dtos.RouteOfAdministrationDto;
using EPharm.Domain.Dtos.SideEffectDto;
using EPharm.Domain.Dtos.StockDto;
using EPharm.Domain.Dtos.UsageWarningDto;
using EPharm.Domain.Dtos.WarehouseDto;

namespace EPharm.Domain.Dtos.ProductDtos;

public class GetFullProductDto
{
    public int Id { get; set; }
    public int PharmaCompanyId { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal StrengthMg { get; set; }
    public int MaxDayFrequency { get; set; }
    public int MaxSupplyInDaysDays { get; set; }
    public string ContraindicationsDescription { get; set; }
    public string StorageConditionDescription { get; set; }

    public int ActiveIngredientsId { get; set; }
    public int SpecialRequirementsId { get; set; }
    public int ManufacturerId { get; set; }
    public int RegulatoryInformationId { get; set; }
    
    public ICollection<GetActiveIngredientDto> ActiveIngredient { get; set; }
    public ICollection<GetDosageFormDto> DosageForms { get; set; }
    public ICollection<GetRouteOfAdministrationDto> RouteOfAdministrations { get; set; }
    public ICollection<GetSideEffectDto> SideEffects { get; set; }
    public ICollection<GetUsageWarningDto> UsageWarnings { get; set; }
    public ICollection<GetAllergyDto> Allergies { get; set; }
    public ICollection<GetIndicationDto> Indications { get; set; }

    public DateTime ManufacturingDate { get; set; }
    public DateTime ExpiryDate { get; set; }

    public int Price { get; set; }
    public int CostPerItem { get; set; }
    public IEnumerable<GetStockDto> Stocks { get; set; }

    public string BatchNumber { get; set; }
    public string Barcode { get; set; }
    public decimal PackagingWeight { get; set; } 
}
