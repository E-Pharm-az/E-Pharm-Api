using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

namespace EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

public interface IManufacturerRepository : IRepository<Manufacturer>
{
    public Task<IEnumerable<Manufacturer>> GetAllCompanyManufacturersAsync(int pharmaCompanyId);
}
