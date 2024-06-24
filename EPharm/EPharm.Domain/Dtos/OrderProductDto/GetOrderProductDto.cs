namespace EPharm.Domain.Dtos.OrderProductDto;

public class GetOrderProductDto
{
    public int Id { get; set; }
    public int WarehouseId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int Frequency { get; set; }
    public int SupplyDuration { get; set; }
}