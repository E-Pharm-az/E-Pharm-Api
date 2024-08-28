using EPharm.Domain.Dtos.ProductDtos;

namespace EPharm.Domain.Dtos.OrderDto;

public class GetOrderProductDto
{
    public GetProductDto Product { get; set; }
    public int WarehouseId { get; set; }
    public int Quantity { get; set; }
    public int Frequency { get; set; }
    public int SupplyDuration { get; set; }
    public decimal TotalPrice { get; set; }
}
