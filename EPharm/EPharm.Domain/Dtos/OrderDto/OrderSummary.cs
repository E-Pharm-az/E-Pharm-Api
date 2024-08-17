namespace EPharm.Domain.Dtos.OrderDto;

public class OrderSummary
{
    public int TotalPrice { get; set; }
    public ICollection<ProductSummary> Products { get; set; } = new List<ProductSummary>();
}
