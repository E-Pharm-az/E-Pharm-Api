using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.Junctions;
using PharmaPortalService.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.JunctionsRepositories;

public class ProductAllergyRepository : Repository<ProductAllergy>, IProductAllergyRepository
{
    protected ProductAllergyRepository(AppDbContext context) : base(context)
    {
    }
}
