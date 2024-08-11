namespace EPharm.Domain.Dtos.OrderDto;

public class GetOrderProductDto
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public int Quantity { get; set; }
    public int Frequency { get; set; }
    public int SupplyDuration { get; set; }
    public double TotalPrice { get; set; }
}
