using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Junctions;

public interface IProductUsageWarningRepository : IRepository<ProductUsageWarning>
{
    public Task InsertProductUsageWarningAsync(int productId, int[] usageWarningsIds);
}
