using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Junctions;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Repositories.Base;

namespace EPharm.Infrastructure.Repositories.Junctions;

public class IndicationProductRepository(AppDbContext context, IIndicationRepository indicationRepository)
    : Repository<IndicationProduct>(context), IIndicationProductRepository
{
    public async Task InsertAsync(int productId, int[] indicationsIds)
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
