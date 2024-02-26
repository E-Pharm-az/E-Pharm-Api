using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class ProductAllergyRepository : Repository<ProductAllergy>, IProductAllergyRepository
{
    protected ProductAllergyRepository(AppDbContext context) : base(context)
    {
    }
}
