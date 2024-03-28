using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EPharm.Domain.Dtos.ProductDtos;

public class CreateProductDto
{
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public int WarehouseId { get; set; }
    public IFormFile? ProductImage { get; set; }
    public decimal StrengthMg { get; set; }
    public int MaxDayFrequency { get; set; }
    public int MaxSupplyInDaysDays { get; set; }
    public string ContraindicationsDescription { get; set; }
    public string StorageConditionDescription { get; set; }
    public int SpecialRequirementsId { get; set; }
    public int ManufacturerId { get; set; }
    public int RegulatoryInformationId { get; set; }
    public DateTime ManufacturingDate { get; set; }
    [Required] public int[] ActiveIngredientsIds { get; set; }
    public int[] AllergiesIds { get; set; }
    [Required] public int[] DosageFormsIds { get; set; }
    [Required] public int[] IndicationsIds { get; set; }
    [Required] public int[] RouteOfAdministrationsIds { get; set; }
    public int[] SideEffectsIds { get; set; }
    public int[] UsageWarningsIds { get; set; }
    [Required] public DateTime ExpiryDate { get; set; }
    public int Stock { get; set; }
    public int Price { get; set; }
    public int CostPerItem { get; set; }
    public string BatchNumber { get; set; }
    public string Barcode { get; set; }
    public decimal PackagingWidth { get; set; }
    public decimal PackagingHeight { get; set; }
    public decimal PackagingLength { get; set; }
    public decimal PackagingWeight { get; set; }
}
