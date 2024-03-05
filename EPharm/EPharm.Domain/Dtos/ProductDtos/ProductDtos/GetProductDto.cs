namespace EPharm.Domain.Dtos.ProductDtos.ProductDtos;

public class GetProductDto
{
    public int Id { get; set; }
    public int PharmaCompanyId { get; set; }
    
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public decimal StrengthMg { get; set; }
    public string ContraindicationsDescription { get; set; }
    public string StorageConditionDescription { get; set; }

    public int SpecialRequirementsId { get; set; }
    public int ManufacturerId { get; set; }
    public int RegulatoryInformationId { get; set; }

    public DateTime ManufacturingDate { get; set; }
    public DateTime ExpiryDate { get; set; }

    public int Stock { get; set; }
    public double Price { get; set; }

    public string BatchNumber { get; set; }
    public string Barcode { get; set; }

    public decimal PackagingWidth { get; set; }
    public decimal PackagingHeight { get; set; }
    public decimal PackagingLength { get; set; }
    public decimal PackagingWeight { get; set; }
}
