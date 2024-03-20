using EPharm.Domain.Dtos.ProductDtos;

namespace EPharm.Domain.Interfaces.Product;

public interface IProductService
{
    public Task<IEnumerable<GetProductDto>> GetAllProductsAsync();
    public Task<IEnumerable<GetProductDto>> GetAllPharmaCompanyProductsAsync(int pharmaCompanyId);
    public Task<GetFullProductDto?> GetProductByIdAsync(int productId);
    public Task<IEnumerable<GetProductDto>> SearchProduct(string parameter);
    public Task<GetProductDto> CreateProductAsync(int pharmaCompanyId, CreateProductDto productDto);
    public Task<bool> UpdateProductAsync(int id, CreateProductDto productDto);
    public Task<bool> DeleteProductAsync(int productId);
}
