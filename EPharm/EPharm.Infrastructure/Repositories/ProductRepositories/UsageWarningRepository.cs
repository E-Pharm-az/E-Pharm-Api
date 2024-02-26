using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class UsageWarningRepository : Repository<UsageWarning>, IUsageWarningRepository
{
    protected UsageWarningRepository(AppDbContext context) : base(context)
    {
    }
}
