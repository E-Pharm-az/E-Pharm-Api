using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class DosageFormRepository : Repository<DosageForm>, IDosageFormRepository
{
    protected DosageFormRepository(AppDbContext context) : base(context)
    {
    }
}
