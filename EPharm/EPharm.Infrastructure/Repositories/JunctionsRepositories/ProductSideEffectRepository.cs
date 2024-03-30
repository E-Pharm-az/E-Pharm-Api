using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

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
