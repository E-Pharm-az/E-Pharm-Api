using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;
using PharmaPortalService.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.ProductRepositories;

public class UsageWarningRepository : Repository<UsageWarning>, IUsageWarningRepository
{
    protected UsageWarningRepository(AppDbContext context) : base(context)
    {
    }
}
