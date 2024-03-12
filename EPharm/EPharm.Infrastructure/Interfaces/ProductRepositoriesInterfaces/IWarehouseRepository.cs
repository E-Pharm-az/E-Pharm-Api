using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

namespace EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

public interface IWarehouseRepository : IRepository<Warehouse>
{
    public Task<IEnumerable<Warehouse>> GetAllCompanyWarehousesAsync(int companyId);
}