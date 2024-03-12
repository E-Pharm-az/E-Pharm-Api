using System.ComponentModel.DataAnnotations;

namespace EPharm.Domain.Dtos.ProductDtos;

public class CreateProductDto
{
    [Required]
    public string ProductName { get; set; }
    
    [Required]
    public string ProductDescription { get; set; }
    
    [Required]
    public int WarehouseId { get; set; }
    
    // public IFormFile[] ProductImages { get; set; }
    
    [Required]
    public decimal StrengthMg { get; set; }
    
    [Required]
    public string ContraindicationsDescription { get; set; }
    
    [Required]
    public string StorageConditionDescription { get; set; }
    
    [Required]
    public int SpecialRequirementsId { get; set; }
    
    [Required]
    public int ManufacturerId { get; set; }
    
    [Required]
    public int RegulatoryInformationId { get; set; }

    [Required]
    public DateTime ManufacturingDate { get; set; }
    
    [Required]
    public int[] ActiveIngredientsIds { get; set; }
    
    public int[] AllergiesIds { get; set; }
    
    [Required]
    public int[] DosageFormsIds { get; set; }
    
    [Required]
    public int[] IndicationsIds { get; set; }
    
    [Required]
    public int[] RouteOfAdministrationsIds { get; set; }
    
    public int[] SideEffectsIds { get; set; }
    
    public int[] UsageWarningsIds { get; set; }
    
    [Required]
    public DateTime ExpiryDate { get; set; }

    public int Stock { get; set; }
    public int Price { get; set; }
    public string BatchNumber { get; set; }
    public string Barcode { get; set; }

    [Required]
    public decimal PackagingWidth { get; set; }
    
    [Required]
    public decimal PackagingHeight { get; set; }
    
    [Required]
    public decimal PackagingLength { get; set; }
    
    [Required]
    public decimal PackagingWeight { get; set; }
}
