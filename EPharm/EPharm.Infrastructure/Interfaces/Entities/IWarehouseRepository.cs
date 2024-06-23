using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Entities;

public interface IWarehouseRepository : IRepository<Warehouse>
{
    public Task<IEnumerable<Warehouse>> GetAllCompanyWarehousesAsync(int companyId);
}
