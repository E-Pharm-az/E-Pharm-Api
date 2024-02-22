using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;
using PharmaPortalService.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.ProductRepositories;

public class DosageFormRepository : Repository<DosageForm>, IDosageFormRepository
{
    protected DosageFormRepository(AppDbContext context) : base(context)
    {
    }
}
