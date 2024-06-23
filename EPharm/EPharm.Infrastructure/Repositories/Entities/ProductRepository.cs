using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Product;
using EPharm.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.Product;

public class ProductRepository(AppDbContext context) : Repository<Context.Entities.ProductEntities.Product>(context), IProductRepository
{
    public async Task<ICollection<Context.Entities.ProductEntities.Product>> GetAlLProductsAsync(int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;

        return await Entities
            .OrderByDescending(product => product.Name)
            .Include(product => product.Stock).ThenInclude(product => product.Warehouse)
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ICollection<Context.Entities.ProductEntities.Product>> GetAlLApprovedProductsAsync(int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;

        return await Entities
            .Where(product => product.IsApproved)
            .OrderByDescending(product => product.Name)
            .Include(product => product.Stock).ThenInclude(product => product.Warehouse)
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Context.Entities.ProductEntities.Product?> GetApprovedProductDetailAsync(int productId) =>
        await Entities
            .Where(product => product.Id == productId)
            .Where(product => product.IsApproved)
            .Include(product => product.Pharmacy)
            .Include(product => product.SpecialRequirement)
            .Include(product => product.Manufacturer)
            .Include(product => product.RegulatoryInformation)
            .Include(product => product.Stock).ThenInclude(product => product.Warehouse)
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

    public async Task<IEnumerable<Context.Entities.ProductEntities.Product>> GetApprovedAllPharmaCompanyProductsAsync(int pharmaCompanyId, int page,
        int pageSize)
    {
        var skip = (page - 1) * pageSize;

        return await Entities.Where(product => product.PharmaCompanyId == pharmaCompanyId)
            .Where(product => product.IsApproved)
            .OrderByDescending(product => product.Name)
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Context.Entities.ProductEntities.Product>> GetAllPharmaCompanyProductsAsync(int pharmaCompanyId, int page,
        int pageSize)
    {
        var skip = (page - 1) * pageSize;

        return await Entities.Where(product => product.PharmaCompanyId == pharmaCompanyId)
            .OrderByDescending(product => product.Name)
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Context.Entities.ProductEntities.Product>> GetApprovedProductsByIdAsync(int[] productIds) =>
        await Entities
            .Where(product => productIds.Contains(product.Id) && product.IsApproved)
            .AsNoTracking().ToListAsync();

}