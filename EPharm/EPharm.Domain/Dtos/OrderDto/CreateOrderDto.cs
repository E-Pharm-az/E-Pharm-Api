namespace EPharm.Domain.Dtos.OrderDto;

public class CreateOrderDto
{
    public string OrderStatus { get; set; }
    public string ShippingAddress { get; set; }
    public int[] ProductIds { get; set; }
}
