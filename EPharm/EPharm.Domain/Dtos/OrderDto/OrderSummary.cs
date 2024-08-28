namespace EPharm.Domain.Dtos.OrderDto;

public class OrderSummary
{
    public decimal TotalPrice { get; set; }
    public ICollection<ProductSummary> Products { get; set; } = new List<ProductSummary>();
}
