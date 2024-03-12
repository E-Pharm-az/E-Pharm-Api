namespace EPharm.Domain.Dtos.ProductImageDto;

public class GetProductImageDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ImageUrl { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
