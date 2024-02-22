using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;
using PharmaPortalService.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.ProductRepositories;

public class SpecialRequirementRepository : Repository<SpecialRequirement>, ISpecialRequirementRepository
{
    protected SpecialRequirementRepository(AppDbContext context) : base(context)
    {
    }
}
