using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class ProductRepository(AppDbContext context) : Repository<Product>(context), IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllProductsWithImages() =>
        await Entities.Include(product => product.ProductImages).AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Product>> GetAllPharmaCompanyProductsAsync(int pharmaCompanyId) =>
        await Entities.Where(product => product.PharmaCompanyId == pharmaCompanyId).Include(product => product.ProductImages).AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Product>> GetProductWithImageById(int productId) =>
        await Entities.Where(product => product.Id == productId).Include(product => product.ProductImages).AsNoTracking().ToListAsync();
}
