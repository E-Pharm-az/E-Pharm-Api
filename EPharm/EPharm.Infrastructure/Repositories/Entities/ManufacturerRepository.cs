using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.Entities;

public class ManufacturerRepository(AppDbContext context) : Repository<Manufacturer>(context), IManufacturerRepository
{
    public async Task<IEnumerable<Manufacturer>> GetAllCompanyManufacturersAsync(int pharmaCompanyId) =>
        await Entities.Where(manufacturer => manufacturer.PharmaCompanyId == pharmaCompanyId).AsNoTracking().ToListAsync();
}
