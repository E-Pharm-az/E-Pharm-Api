using EPharm.Domain.Dtos.ProductDtos.ProductDtos;

namespace EPharm.Domain.Interfaces.Product;

public interface IProductService
{
    public Task<IEnumerable<GetProductDto>> GetAllProductsAsync();
    public Task<GetProductDto?> GetProductByIdAsync(int productId);
    public Task<GetProductDto> CreateProductAsync(CreateProductDto productDto);
    public Task<bool> UpdateProductAsync(CreateProductDto productDto);
    public Task<bool> DeleteProductAsync(int productId);
}
