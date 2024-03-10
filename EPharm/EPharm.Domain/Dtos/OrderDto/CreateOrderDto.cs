namespace EPharm.Domain.Dtos.OrderDto;

public class CreateOrderDto
{
    public int PharmaCompanyId { get; set; }
    public int Quantity { get; set; }
    public string OrderStatus { get; set; }
    public double TotalPrice { get; set; }
    public string ShippingAddress { get; set; }
    public int[] ProductIds { get; set; }
    public string UserId { get; set; }
    public int WarehouseId { get; set; } 
}
