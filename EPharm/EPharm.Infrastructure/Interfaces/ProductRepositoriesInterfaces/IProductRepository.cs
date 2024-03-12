using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

namespace EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

public interface IProductRepository : IRepository<Product>
{
    public Task<IEnumerable<Product>> GetAllProductsWithImages();
    public Task<IEnumerable<Product>> GetAllPharmaCompanyProductsAsync(int pharmaCompanyId);
    public Task<IEnumerable<Product>> GetProductWithImageById(int productId);
}
