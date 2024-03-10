using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

namespace EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

public interface IProductDosageFormRepository : IRepository<ProductDosageForm>
{
    public Task InsertProductDosageFormAsync(int productId, int[] dosageFormsIds);
}
