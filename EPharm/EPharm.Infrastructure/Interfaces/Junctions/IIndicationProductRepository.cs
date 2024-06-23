using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

public interface IIndicationProductRepository : IRepository<IndicationProduct>
{
    public Task InsertIndicationProductAsync(int productId, int[] indicationsIds);
}
