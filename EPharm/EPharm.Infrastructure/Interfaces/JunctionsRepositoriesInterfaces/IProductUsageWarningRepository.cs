using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

namespace EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

public interface IProductUsageWarningRepository : IRepository<ProductUsageWarning>
{
    public Task InsertProductUsageWarningAsync(int productId, int[] usageWarningsIds);
}
