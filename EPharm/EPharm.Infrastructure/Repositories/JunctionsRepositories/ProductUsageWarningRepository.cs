using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class ProductUsageWarningRepository(AppDbContext context, IUsageWarningRepository usageWarningRepository)
    : Repository<ProductUsageWarning>(context), IProductUsageWarningRepository
{
    public async Task InsertProductUsageWarningAsync(int productId, int[] usageWarningsIds)
    {
        foreach (var usageWarningsId in usageWarningsIds)
        {
            var usageWarning = await usageWarningRepository.GetByIdAsync(usageWarningsId);
            
            if (usageWarning is null)
                throw new ArgumentException("Usage warning not found");
            
            await Entities.AddAsync(
                new ProductUsageWarning
                {
                    ProductId = productId,
                    UsageWarningId = usageWarningsId
                }
            );
        }
    }
}
