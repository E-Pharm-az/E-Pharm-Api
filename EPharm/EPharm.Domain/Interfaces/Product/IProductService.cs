using EPharm.Domain.Dtos.ProductDtos;
using EPharm.Domain.Dtos.ProductDtos.ProductDtos;

namespace EPharm.Domain.Interfaces.Product;

public interface IProductService
{
    public Task<IEnumerable<GetProductDto>> GetAllProductsAsync();
    public Task<IEnumerable<GetProductDto>> GetAllPharmaCompanyProductsAsync(int pharmaCompanyId);
    public Task<GetProductDto?> GetProductByIdAsync(int productId);
    public Task<GetProductDto> CreateProductAsync(int pharmaCompanyId, CreateProductDto productDto);
    public Task<bool> UpdateProductAsync(int id, CreateProductDto productDto);
    public Task<bool> DeleteProductAsync(int productId);
}
