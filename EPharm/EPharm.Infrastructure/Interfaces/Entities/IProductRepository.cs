using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Entities;

public interface IProductRepository : IRepository<Infrastructure.Entities.ProductEntities.Product>
{
    public Task<ICollection<Infrastructure.Entities.ProductEntities.Product>> GetAlLProductsAsync(int page, int pageSize);
    public Task<ICollection<Infrastructure.Entities.ProductEntities.Product>> GetAlLApprovedProductsAsync(int page, int pageSize);
    public Task<Infrastructure.Entities.ProductEntities.Product?> GetApprovedProductDetailAsync(int productId);
    public Task<IEnumerable<Infrastructure.Entities.ProductEntities.Product>> GetApprovedAllPharmaCompanyProductsAsync(int pharmaCompanyId, int page, int pageSize);
    public Task<IEnumerable<Infrastructure.Entities.ProductEntities.Product>> GetAllPharmaCompanyProductsAsync(int pharmaCompanyId, int page, int pageSize);
    public Task<IEnumerable<Infrastructure.Entities.ProductEntities.Product>> GetApprovedProductsByIdAsync(int[] productIds);
}
