using System.ComponentModel.DataAnnotations;
using EPharm.Domain.Dtos.StockDto;
using EPharm.Domain.Validation;
using Microsoft.AspNetCore.Http;
namespace EPharm.Domain.Dtos.ProductDtos;

public class CreateProductDto
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    
    [AllowedExtensions([".jpg", ".png"])]
    [MaxFileSize(10 * 1024 * 1024)]
    public IFormFile? Image { get; set; }
    public decimal StrengthMg { get; set; }
    public int? MaxDayFrequency { get; set; }
    public int? MaxSupplyInDaysDays { get; set; }
    public string? ContraindicationsDescription { get; set; }
    public string? StorageConditionDescription { get; set; }
    public int? SpecialRequirementsId { get; set; }
    public int ManufacturerId { get; set; }
    public int? RegulatoryInformationId { get; set; }
    public int[] ActiveIngredientsIds { get; set; }
    public int[]? AllergiesIds { get; set; }
    public int[] DosageFormsIds { get; set; }
    public int[]? IndicationsIds { get; set; }
    public int[] RouteOfAdministrationsIds { get; set; }
    public int[]? SideEffectsIds { get; set; }
    public int[]? UsageWarningsIds { get; set; }
    public DateTime ManufacturingDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public decimal Price { get; set; }
    public int CostPerItem { get; set; }
    public List<CreateStockDto> Stocks { get; set; }
    public string? BatchNumber { get; set; }
    public string? Barcode { get; set; }
    public decimal PackagingWeight { get; set; }
}
