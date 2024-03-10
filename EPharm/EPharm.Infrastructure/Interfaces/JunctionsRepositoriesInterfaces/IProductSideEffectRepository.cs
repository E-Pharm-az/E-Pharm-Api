using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

namespace EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

public interface IProductSideEffectRepository : IRepository<ProductSideEffect>
{
    public Task InsertProductSideEffectAsync(int productId, int[] sideEffectsIds);
}
