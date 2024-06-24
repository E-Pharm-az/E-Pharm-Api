using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Entities;

public interface IManufacturerRepository : IRepository<Manufacturer>
{
    public Task<IEnumerable<Manufacturer>> GetAllCompanyManufacturersAsync(int pharmaCompanyId);
}
