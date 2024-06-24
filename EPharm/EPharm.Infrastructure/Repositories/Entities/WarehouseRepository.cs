using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.Entities;

public class WarehouseRepository(AppDbContext context) : Repository<Warehouse>(context), IWarehouseRepository
{
    public async Task<IEnumerable<Warehouse>> GetAllCompanyWarehousesAsync(int companyId) =>
        await Entities.Where(w => w.PharmaCompanyId == companyId).ToListAsync();
}
