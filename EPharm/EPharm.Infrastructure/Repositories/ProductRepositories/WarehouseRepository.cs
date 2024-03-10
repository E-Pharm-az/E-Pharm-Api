using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class WarehouseRepository(AppDbContext context) : Repository<Warehouse>(context), IWarehouseRepository
{
    public async Task<IEnumerable<Warehouse>> GetAllCompanyWarehousesAsync(int companyId) =>
        await Entities.Where(w => w.PharmaCompanyId == companyId).ToListAsync();
}
