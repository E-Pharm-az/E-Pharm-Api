using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Product;

public interface IManufacturerRepository : IRepository<Manufacturer>
{
    public Task<IEnumerable<Manufacturer>> GetAllCompanyManufacturersAsync(int pharmaCompanyId);
}
