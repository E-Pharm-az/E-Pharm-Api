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

    public async Task<Product?> GetDetailProductByIdAsync(int id)
    {
        var product = await Entities
            .Where(product => product.Id == id)
            .Include(product => product.Pharmacy)
            .Include(product => product.SpecialRequirement)
            .Include(product => product.Manufacturer)
            .Include(product => product.RegulatoryInformation)
            .AsNoTracking()
            .SingleOrDefaultAsync();

        if (product != null)
        {
            await LoadRelatedEntities(product);
        }

        return product;
    }
    
    private async Task LoadRelatedEntities(Product product)
    {
        product.Stock = await context.WarehouseProduct
            .Where(s => s.ProductId == product.Id)
            .Include(s => s.Warehouse)
            .ToListAsync();

        product.ActiveIngredients = await context.ProductActiveIngredients
            .Where(ai => ai.ProductId == product.Id)
            .Include(ai => ai.ActiveIngredient)
            .ToListAsync();

        product.DosageForms = await context.ProductDosageForms
            .Where(df => df.ProductId == product.Id)
            .Include(df => df.DosageForm)
            .ToListAsync();
        
        product.RouteOfAdministrations = await context.ProductRouteOfAdministrations
            .Where(roa => roa.ProductId == product.Id)
            .Include(roa => roa.RouteOfAdministration)
            .ToListAsync();

        product.SideEffects = await context.ProductSideEffects
            .Where(se => se.ProductId == product.Id)
            .Include(se => se.SideEffect)
            .ToListAsync();

        product.UsageWarnings = await context.ProductUsageWarnings
            .Where(uw => uw.ProductId == product.Id)
            .Include(uw => uw.UsageWarning)
            .ToListAsync();

        product.Allergies = await context.ProductAllergies
            .Where(a => a.ProductId == product.Id)
            .Include(a => a.Allergy)
            .ToListAsync();

        product.Indications = await context.IndicationProducts
            .Where(i => i.ProductId == product.Id)
            .Include(i => i.Indication)
            .ToListAsync();
    }

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
            .Include(product => product.Stock).ThenInclude(product => product.Warehouse)
            .ToListAsync();
}
