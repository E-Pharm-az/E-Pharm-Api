using EPharm.Domain.Dtos.StockDto;

namespace EPharm.Domain.Dtos.ProductDtos;

public class GetProductDto
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
    public int SpecialRequirementsId { get; set; }
    public int ManufacturerId { get; set; }
    public int RegulatoryInformationId { get; set; }

    public int[] ActiveIngredientsIds{ get; set; }
    public int[] AllergiesIds { get; set; }
    public int[] DosageFormsIds { get; set; }
    public int[] IndicationsIds { get; set; }
    public int[] RouteOfAdministrationsIds { get; set; }
    public int[] SideEffectsIds { get; set; }
    public int[] UsageWarningsIds { get; set; }

    public DateTime ManufacturingDate { get; set; }
    public DateTime ExpiryDate { get; set; }

    public int Price { get; set; }
    public int CostPerItem { get; set; }
    public ICollection<GetStockDto> Stock { get; set; }

    public string BatchNumber { get; set; }
    public string Barcode { get; set; }
    public decimal PackagingWeight { get; set; }
}
