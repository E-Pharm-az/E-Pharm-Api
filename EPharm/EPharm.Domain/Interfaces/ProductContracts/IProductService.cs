using EPharm.Domain.Dtos.ProductDtos;

namespace EPharm.Domain.Interfaces.ProductContracts;

public interface IProductService
{
    public Task<IEnumerable<GetProductDto>> GetAllProductsAsync(int page);
    public Task<IEnumerable<GetProductDto>> SearchProduct(string parameter, int page);
    public Task<IEnumerable<GetProductDto>> GetAllPharmaCompanyProductsAsync(int pharmaCompanyId, int page);
    public Task<GetDetailProductDto?> GetProductByIdAsync(int productId);
    public Task ApproveProductAsync(string adminId, int productId);
    public Task<GetProductDto> CreateProductAsync(int pharmaCompanyId, CreateProductDto productDto);
    public Task<bool> UpdateProductAsync(int pharmacyId, int id, CreateProductDto productDto);
    public Task<bool> DeleteProductAsync(int pharmacyId, int productId);
}
