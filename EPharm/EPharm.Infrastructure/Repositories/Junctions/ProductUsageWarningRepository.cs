using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Junctions;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Repositories.Base;

namespace EPharm.Infrastructure.Repositories.Junctions;

public class ProductUsageWarningRepository(AppDbContext context, IUsageWarningRepository usageWarningRepository)
    : Repository<ProductUsageWarning>(context), IProductUsageWarningRepository
{
    public async Task InsertAsync(int productId, int[] usageWarningsIds)
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

        await base.SaveChangesAsync();
    }
}
