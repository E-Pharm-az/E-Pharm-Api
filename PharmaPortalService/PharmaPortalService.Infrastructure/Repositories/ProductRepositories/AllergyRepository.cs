using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;
using PharmaPortalService.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.ProductRepositories;

public class AllergyRepository : Repository<Allergy>, IAllergyRepository
{
    protected AllergyRepository(AppDbContext context) : base(context)
    {
    }
}
