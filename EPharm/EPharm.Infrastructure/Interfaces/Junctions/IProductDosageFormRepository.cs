using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Junctions;

public interface IProductDosageFormRepository : IRepository<ProductDosageForm>
{
    public Task InsertProductDosageFormAsync(int productId, int[] dosageFormsIds);
}
