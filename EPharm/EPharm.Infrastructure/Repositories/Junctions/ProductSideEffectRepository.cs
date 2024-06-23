using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Junctions;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Repositories.Base;

namespace EPharm.Infrastructure.Repositories.Junctions;

public class ProductSideEffectRepository(AppDbContext context, ISideEffectRepository sideEffectRepository)
    : Repository<ProductSideEffect>(context), IProductSideEffectRepository
{
    public async Task InsertProductSideEffectAsync(int productId, int[] sideEffectsIds)
    {
        foreach (var sideEffectsId in sideEffectsIds)
        {
            var sideEffect = await sideEffectRepository.GetByIdAsync(sideEffectsId);

            if (sideEffect is null)
                throw new ArgumentException("Side effect not found");
            
            await Entities.AddAsync(
                new ProductSideEffect
                {
                    ProductId = productId,
                    SideEffectId = sideEffectsId
                }
            );
        }
        
        await base.SaveChangesAsync();
    }
}
