namespace EPharm.Domain.Dtos.OrderDto;

public class CreateOrderDto
{
    public string? UserId { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string ShippingAddress { get; set; }
    public int[] ProductIds { get; set; }
}
