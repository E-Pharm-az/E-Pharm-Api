using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class IndicationProductRepository(AppDbContext context, IIndicationRepository indicationRepository)
    : Repository<IndicationProduct>(context), IIndicationProductRepository
{
    public async Task InsertIndicationProductAsync(int productId, int[] indicationsIds)
    {
        foreach (var indicationsId in indicationsIds)
        {
            var indication = await indicationRepository.GetByIdAsync(indicationsId);
            
            if (indication is null)
                throw new ArgumentException("Indication not found");
            
            await Entities.AddAsync(
                new IndicationProduct
                {
                    ProductId = productId,
                    IndicationId = indicationsId
                }
            );
        }

        await base.SaveChangesAsync();
    }
}
