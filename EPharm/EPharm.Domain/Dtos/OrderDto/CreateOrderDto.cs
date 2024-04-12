using EPharm.Domain.Dtos.OrderProductDto;

namespace EPharm.Domain.Dtos.OrderDto;

public class CreateOrderDto
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FullName { get; set; }
    public string ShippingAddress { get; set; }
    public ICollection<CreateOrderProductDto> Products { get; set; }
}
