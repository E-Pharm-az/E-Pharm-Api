using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Junctions;

public interface IProductSideEffectRepository : IRepository<ProductSideEffect>
{
    public Task InsertProductSideEffectAsync(int productId, int[] sideEffectsIds);
}
