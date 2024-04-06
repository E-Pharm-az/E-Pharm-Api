namespace EPharm.Domain.Dtos.OrderDto;

public class GetOrderDto
{
    public int Id { get; set; }
    public string TrackingId { get; set; }
    public string OrderStatus { get; set; }
    public int TotalPrice { get; set; }
    public string ShippingAddress { get; set; }
    public int[] ProductIds { get; set; }
    public string? UserId { get; set; }
    public int PharmaCompanyId { get; set; }
    public int WarehouseId { get; set; }
}
