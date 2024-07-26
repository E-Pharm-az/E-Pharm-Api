using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Junctions;

public interface IProductUsageWarningRepository : IRepository<ProductUsageWarning>
{
    public Task InsertAsync(int productId, int[] usageWarningsIds);
}
