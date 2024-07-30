using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Entities;

public interface IProductRepository : IRepository<Product>
{
    public Task<ICollection<Product>> GetAlLProductsAsync(int page, int pageSize);
    public Task<ICollection<Product>> GetAlLApprovedProductsAsync(int page, int pageSize);
    public Task<Product?> GetFullByIdAsync(int id);
    public Task<IEnumerable<Product>> GetApprovedAllPharmaCompanyProductsAsync(int pharmaCompanyId, int page, int pageSize);
    public Task<IEnumerable<Product>> GetAllPharmaCompanyProductsAsync(int pharmaCompanyId, int page, int pageSize);
    public Task<IEnumerable<Product>> GetApprovedProductsByIdAsync(int[] productIds);
}
