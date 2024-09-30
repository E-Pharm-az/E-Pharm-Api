namespace EPharm.Domain.Dtos.OrderDto;

public class GetOrderDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ICollection<GetOrderProductDto> OrderProducts { get; set; }
    public string TrackingId { get; set; }
    public string Status { get; set; }
    public decimal TotalPrice { get; set; }
    public string Address { get; set; }
    public string District { get; set; }
    public string City { get; set; }
    public int Zip { get; set; }
    public DateTime CreatedAt { get; set; }
}
