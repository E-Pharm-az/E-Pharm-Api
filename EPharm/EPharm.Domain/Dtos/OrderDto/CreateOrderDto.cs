using EPharm.Domain.Dtos.OrderProductDto;

namespace EPharm.Domain.Dtos.OrderDto;

public class CreateOrderDto
{
    public string UserId { get; set; }
    public string Address { get; set; }
    public string District { get; set; }
    public string City { get; set; }
    public int Zip { get; set; }
    public string Currency { get; set; }
    public ICollection<CreateOrderProductDto> Products { get; set; }
}
