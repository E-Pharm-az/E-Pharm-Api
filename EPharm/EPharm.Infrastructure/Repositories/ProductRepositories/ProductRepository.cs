using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class ProductRepository(AppDbContext context) : Repository<Product>(context), IProductRepository
{
    public async Task<ICollection<Product>> GetAlLProductsAsync(int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;

        return await Entities
            .OrderByDescending(product => product.Name)
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Product?> GetFullProductDetailAsync(int productId) =>
        await Entities
            .Where(product => product.Id == productId)
            .Include(product => product.PharmaCompany)
            .Include(product => product.SpecialRequirement)
            .Include(product => product.Manufacturer)
            .Include(product => product.RegulatoryInformation)
            .Include(product => product.WarehouseProducts).ThenInclude(product => product.Warehouse)
            .Include(product => product.ActiveIngredients).ThenInclude(product => product.ActiveIngredient)
            .Include(product => product.DosageForms).ThenInclude(product => product.DosageForm)
            .Include(product => product.RouteOfAdministrations).ThenInclude(product => product.RouteOfAdministration)
            .Include(product => product.SideEffects).ThenInclude(product => product.SideEffect)
            .Include(product => product.UsageWarnings).ThenInclude(product => product.UsageWarning)
            .Include(product => product.Allergies).ThenInclude(product => product.Allergy)
            .Include(product => product.Allergies).ThenInclude(product => product.Allergy)
            .Include(product => product.Allergies).ThenInclude(product => product.Allergy)
            .AsNoTracking()
            .SingleOrDefaultAsync();

    public async Task<IEnumerable<Product>> GetAllPharmaCompanyProductsAsync(int pharmaCompanyId) =>
        await Entities.Where(product => product.PharmaCompanyId == pharmaCompanyId).AsNoTracking().ToListAsync();
}
