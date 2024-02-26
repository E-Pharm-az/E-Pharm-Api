using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class ProductUsageWarningRepository : Repository<ProductUsageWarning>, IProductUsageWarningRepository
{
    protected ProductUsageWarningRepository(AppDbContext context) : base(context)
    {
    }
}
