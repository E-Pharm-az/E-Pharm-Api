using Microsoft.AspNetCore.Http;

namespace EPharm.Domain.Dtos.ProductImageDto;

public class CreateProductImageDto
{
    public int ProductId { get; set; }
    public IFormFile Image { get; set; }
}
