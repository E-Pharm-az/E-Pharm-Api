using EPharm.Domain.Dtos.UserDto;

namespace EPharm.Domain.Dtos.OrderDto;

public class GetOrderDto
{
    public int OrderId { get; set; }
    public GetUserDto User { get; set; }
    public ICollection<GetOrderProductDto> Products { get; set; }
    public string TrackingId { get; set; }
    public string OrderStatus { get; set; }
    public int TotalPrice { get; set; }
    public string Address { get; set; }
    public string District { get; set; }
    public string City { get; set; }
    public int Zip { get; set; }
    public int PharmacyId { get; set; }
    public int WarehouseId { get; set; }
}
