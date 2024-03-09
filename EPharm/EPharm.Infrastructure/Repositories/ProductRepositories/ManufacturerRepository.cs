using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class ManufacturerRepository(AppDbContext context) : Repository<Manufacturer>(context), IManufacturerRepository
{
    public async Task<IEnumerable<Manufacturer>> GetAllCompanyManufacturersAsync(int pharmaCompanyId) =>
        await Entities.Where(manufacturer => manufacturer.PharmaCompanyId == pharmaCompanyId).AsNoTracking().ToListAsync();
}
