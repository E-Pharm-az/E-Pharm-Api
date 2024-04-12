using EPharm.Domain.Dtos.StockDto;

namespace EPharm.Domain.Dtos.ProductDtos;

public class GetMinimalProductDto
{
    public int Id { get; set; }
    public int PharmaCompanyId { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal StrengthMg { get; set; }
    public int RegulatoryInformationId { get; set; }

    public DateTime ManufacturingDate { get; set; }
    public DateTime ExpiryDate { get; set; }

    public int Price { get; set; }
    public ICollection<GetStockDto> Stock { get; set; }
}
