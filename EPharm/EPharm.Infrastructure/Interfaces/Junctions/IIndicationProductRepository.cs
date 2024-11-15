using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Junctions;

public interface IIndicationProductRepository : IRepository<IndicationProduct>
{
    public Task InsertAsync(int productId, int[] indicationsIds);
}
