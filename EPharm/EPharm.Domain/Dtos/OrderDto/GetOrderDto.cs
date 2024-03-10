namespace EPharm.Domain.Dtos.OrderDto;

public class GetOrderDto
{
    public int Id { get; set; }
    public string TrackingId { get; set; }
    public int Quantity { get; set; }
    public string OrderStatus { get; set; }
    public double TotalPrice { get; set; }
    public string ShippingAddress { get; set; }
    public int ProductId { get; set; }
    public string UserId { get; set; }
    public int PharmaCompanyId { get; set; }
    public int WarehouseId { get; set; }
}
