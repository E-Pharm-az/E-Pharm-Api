using EPharm.Infrastructure.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.Base;

namespace EPharm.Infrastructure.Interfaces.Junctions;

public interface IProductAllergyRepository : IRepository<ProductAllergy>
{
    public Task InsertProductAllergyAsync(int productId, int[] allergiesIds);
}
