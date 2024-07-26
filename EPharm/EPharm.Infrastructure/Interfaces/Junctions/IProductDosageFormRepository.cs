using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Junctions;

public interface IProductDosageFormRepository : IRepository<ProductDosageForm>
{
    public Task InsertAsync(int productId, int[] dosageFormsIds);
}
