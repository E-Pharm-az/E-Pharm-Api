using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;
using PharmaPortalService.Infrastructure.Interfaces.ProductRepositoriesInterfaces;

namespace PharmaPortalService.Infrastructure.Repositories.ProductRepositories;

public class IndicationRepository : Repository<Indication>, IIndicationRepository
{
    protected IndicationRepository(AppDbContext context) : base(context)
    {
    }
}
