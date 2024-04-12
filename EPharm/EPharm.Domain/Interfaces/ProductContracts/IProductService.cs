using EPharm.Domain.Dtos.ProductDtos;

namespace EPharm.Domain.Interfaces.ProductContracts;

public interface IProductService
{
    public Task<IEnumerable<GetMinimalProductDto>> SearchProduct(string parameter, int page);
    public Task<IEnumerable<GetMinimalProductDto>> GetAllProductsAsync();
    public Task<IEnumerable<GetMinimalProductDto>> GetAllPharmaCompanyProductsAsync(int pharmaCompanyId, int page);
    public Task<GetFullProductDto?> GetProductByIdAsync(int productId);
    public Task<GetMinimalProductDto> CreateProductAsync(int pharmaCompanyId, CreateProductDto productDto);
    public Task<bool> UpdateProductAsync(int id, CreateProductDto productDto);
    public Task<bool> DeleteProductAsync(int productId);
}
