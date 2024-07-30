using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.Entities;

public class ProductRepository(AppDbContext context) : Repository<Product>(context), IProductRepository
{
    public async Task<ICollection<Product>> GetAlLProductsAsync(int page, int pageSize)
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

    public async Task<ICollection<Product>> GetAlLApprovedProductsAsync(int page, int pageSize)
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

    public async Task<Product?> GetFullByIdAsync(int id) =>
        await Entities 
            .Where(product => product.Id == id)
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
            .Include(product => product.Indications).ThenInclude(product => product.Indication)
            .AsNoTracking()
            .SingleOrDefaultAsync();

    public async Task<IEnumerable<Product>> GetApprovedAllPharmaCompanyProductsAsync(int pharmaCompanyId, int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;

        return await Entities.Where(product => product.PharmacyId == pharmaCompanyId)
            .Where(product => product.IsApproved)
            .OrderByDescending(product => product.Name)
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllPharmaCompanyProductsAsync(int pharmaCompanyId, int page,
        int pageSize)
    {
        var skip = (page - 1) * pageSize;

        return await Entities.Where(product => product.PharmacyId == pharmaCompanyId)
            .OrderByDescending(product => product.Name)
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetApprovedProductsByIdAsync(int[] productIds) =>
        await Entities
            .Where(product => productIds.Contains(product.Id) && product.IsApproved)
            .AsNoTracking().ToListAsync();
}