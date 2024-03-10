using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.BaseRepositoriesInterfaces;

namespace EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

public interface IProductAllergyRepository : IRepository<ProductAllergy>
{
    public Task InsertProductAllergyAsync(int productId, int[] allergiesIds);
}
