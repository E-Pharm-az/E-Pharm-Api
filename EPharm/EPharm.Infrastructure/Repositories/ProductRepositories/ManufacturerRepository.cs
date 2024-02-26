using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class ManufacturerRepository : Repository<Manufacturer>, IManufacturerRepository
{
    protected ManufacturerRepository(AppDbContext context) : base(context)
    {
    }
}
