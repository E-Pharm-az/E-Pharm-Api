using EPharm.Domain.Dtos.OrderProductDto;

namespace EPharm.Domain.Dtos.OrderDto;

public class GetOrderDto
{
    public int Id { get; set; }
    public string? UserId { get; set; }

    public ICollection<GetOrderProductDto> Products { get; set; }
    public string TrackingId { get; set; }
    public string OrderStatus { get; set; }
    public int TotalPrice { get; set; }
    public string ShippingAddress { get; set; }
    public int PharmacyId { get; set; }
    public int WarehouseId { get; set; }

    // TODO: If the following fields are null, then substitute them with the information from the identity user.
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
