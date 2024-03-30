using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

namespace EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

public interface IProductRepository : IRepository<Product>
{
    public Task<ICollection<Product>> GetAlLProductsAsync(int page, int pageSize);
    public Task<Product?> GetFullProductDetailAsync(int productId);
    public Task<IEnumerable<Product>> GetAllPharmaCompanyProductsAsync(int pharmaCompanyId);
}
