using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

namespace EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

public interface IProductRepository : IRepository<Product>
{
    public Task<ICollection<Product>> GetAlLProductsAsync(int page, int pageSize);
    public Task<ICollection<Product>> GetAlLApprovedProductsAsync(int page, int pageSize);
    public Task<Product?> GetApprovedProductDetailAsync(int productId);
    public Task<IEnumerable<Product>> GetApprovedAllPharmaCompanyProductsAsync(int pharmaCompanyId, int page, int pageSize);
    public Task<IEnumerable<Product>> GetAllPharmaCompanyProductsAsync(int pharmaCompanyId, int page, int pageSize);
    public Task<IEnumerable<Product>> GetApprovedProductsByIdAsync(int[] productIds);
}
