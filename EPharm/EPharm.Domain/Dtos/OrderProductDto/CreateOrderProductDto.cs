namespace EPharm.Domain.Dtos.OrderProductDto;

public class CreateOrderProductDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int? Frequency { get; set; }
    public int? SupplyDuration { get; set; }
}
