namespace EPharm.Domain.Dtos.ProductDtos;

public class GetProductDto
{
    public int Id { get; set; }
    public int PharmaCompanyId { get; set; }
    public int[] WarehouseIds { get; set; }
    
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

    public DateTime ManufacturingDate { get; set; }
    public DateTime ExpiryDate { get; set; }

    public int Stock { get; set; }
    public int Price { get; set; }

    public string BatchNumber { get; set; }
    public string Barcode { get; set; }
    public decimal PackagingWeight { get; set; }
}
