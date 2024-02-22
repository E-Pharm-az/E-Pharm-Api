using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;
using PharmaPortalService.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.JunctionsRepositories;

public class ProductUsageWarningRepository : Repository<ProductUsageWarning>, IProductUsageWarningRepository
{
    protected ProductUsageWarningRepository(AppDbContext context) : base(context)
    {
    }
}
