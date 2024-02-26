using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class AllergyRepository : Repository<Allergy>, IAllergyRepository
{
    protected AllergyRepository(AppDbContext context) : base(context)
    {
    }
}
