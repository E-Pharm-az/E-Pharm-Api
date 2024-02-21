using PharmaPortalService.Infrastructure.Context.Entities.Base;

namespace PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

public class ProductImage : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public string ImageUrl { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
