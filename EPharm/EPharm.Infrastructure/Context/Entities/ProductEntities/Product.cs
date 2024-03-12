using EPharm.Infrastructure.Context.Entities.Base;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;

namespace EPharm.Infrastructure.Context.Entities.ProductEntities;

public class Product : BaseEntity
{
    public int PharmaCompanyId { get; set; }
    public PharmaCompany PharmaCompany { get; set; }
    
    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; }
    
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public decimal StrengthMg { get; set; }
    public string ContraindicationsDescription { get; set; }
    public string StorageConditionDescription { get; set; }
    
    public int SpecialRequirementsId { get; set; }
    public SpecialRequirement SpecialRequirement { get; set; }
    
    public int ManufacturerId { get; set; }
    public Manufacturer Manufacturer { get; set; }
    
    public int RegulatoryInformationId { get; set; }
    public RegulatoryInformation RegulatoryInformation { get; set; }
    public DateTime ManufacturingDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    
    public int Stock { get; set; }
    public int Price { get; set; }
    
    public string BatchNumber { get; set; }
    public string Barcode { get; set; }
    
    public ICollection<ProductActiveIngredient> ActiveIngredients { get; set; }
    public ICollection<ProductDosageForm> DosageForms { get; set; }
    public ICollection<ProductRouteOfAdministration> RouteOfAdministrations { get; set; }
    public ICollection<ProductSideEffect> SideEffects { get; set; }
    public ICollection<ProductUsageWarning> UsageWarnings { get; set; }
    public ICollection<ProductAllergy> Allergies { get; set; }
    public ICollection<IndicationProduct> Indications { get; set; }
    public ICollection<ProductImage> ProductImages { get; set; }
    public ICollection<OrderProduct> OrderProducts;
    
    public decimal PackagingWidth { get; set; }
    public decimal PackagingHeight { get; set; }
    public decimal PackagingLength { get; set; }
    public decimal PackagingWeight { get; set; }

    public DateTime CreatedAt { get; set; }
}
