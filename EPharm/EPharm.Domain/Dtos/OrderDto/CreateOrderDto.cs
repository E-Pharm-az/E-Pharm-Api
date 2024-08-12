using EPharm.Domain.Dtos.OrderProductDto;

namespace EPharm.Domain.Dtos.OrderDto;

public class CreateOrderDto
{
    public string UserId { get; set; }
    public string ShippingAddress { get; set; }
    public string Currency { get; set; }
    public ICollection<CreateOrderProductDto> Products { get; set; }
}
