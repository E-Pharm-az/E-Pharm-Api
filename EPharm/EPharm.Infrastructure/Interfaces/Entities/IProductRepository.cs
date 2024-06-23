using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Product;

public interface IProductRepository : IRepository<Context.Entities.ProductEntities.Product>
{
    public Task<ICollection<Context.Entities.ProductEntities.Product>> GetAlLProductsAsync(int page, int pageSize);
    public Task<ICollection<Context.Entities.ProductEntities.Product>> GetAlLApprovedProductsAsync(int page, int pageSize);
    public Task<Context.Entities.ProductEntities.Product?> GetApprovedProductDetailAsync(int productId);
    public Task<IEnumerable<Context.Entities.ProductEntities.Product>> GetApprovedAllPharmaCompanyProductsAsync(int pharmaCompanyId, int page, int pageSize);
    public Task<IEnumerable<Context.Entities.ProductEntities.Product>> GetAllPharmaCompanyProductsAsync(int pharmaCompanyId, int page, int pageSize);
    public Task<IEnumerable<Context.Entities.ProductEntities.Product>> GetApprovedProductsByIdAsync(int[] productIds);
}
